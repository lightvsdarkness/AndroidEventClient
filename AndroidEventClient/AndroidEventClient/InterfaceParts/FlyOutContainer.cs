using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Animation;
using Android.Graphics;
using Android.Graphics.Drawables;
using AEC;

namespace flyoutmenu
{
    public class FlyOutContainer : FrameLayout
    {
        bool _opened;
        int _contentOffsetX;
        ValueAnimator _animator;
        readonly ITimeInterpolator _interpolator = new SmoothInterpolator();
        VelocityTracker _velocityTracker;
        bool _stateBeforeTracking;
        bool _isTracking;
        bool _preTracking;
        int _startX = -1, _startY = -1;

        // const int BezelArea = 30; //dip
        const int MaxOverlayAlpha = 170;
        const float ParallaxSpeedRatio = 0.25f;

        int _touchSlop;
        int _pagingTouchSlop;
        int _minFlingVelocity;
        int _maxFlingVelocity;

        GradientDrawable _shadowDrawable;
        Paint _overlayPaint;

        public FlyOutContainer(Context context) :
            base(context)
        {
            Initialize();
        }

        public FlyOutContainer(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public FlyOutContainer(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            var config = ViewConfiguration.Get(Context);
            _touchSlop = config.ScaledTouchSlop;
            _pagingTouchSlop = config.ScaledPagingTouchSlop;
            _minFlingVelocity = config.ScaledMinimumFlingVelocity;
            _maxFlingVelocity = config.ScaledMaximumFlingVelocity;

            const int baseShadowColor = 0;
            var shadowColors = new[] {
                                Color.Argb (0x90, baseShadowColor, baseShadowColor, baseShadowColor).ToArgb (),
                                Color.Argb (0, baseShadowColor, baseShadowColor, baseShadowColor).ToArgb ()
                        };
            _shadowDrawable = new GradientDrawable(GradientDrawable.Orientation.RightLeft,
                                                        shadowColors);
            _overlayPaint = new Paint
            {
                Color = Color.Black,
                AntiAlias = true
            };
        }

        View ContentView
        {
            get { return FindViewById(Resource.Id.FlyOutContent); }
        }

        View MenuView
        {
            get { return FindViewById(Resource.Id.FlyOutMenu); }
        }

        int MaxOffset
        {
            get { return MenuView.Width; }
        }

        public bool Opened
        {
            get { return _opened; }
            set { SetOpened(value, false); }
        }

        public bool AnimatedOpened
        {
            get { return _opened; }
            set { SetOpened(value); }
        }

        // very bad code!
        public void ExternalyClosed()
        {
            _contentOffsetX = 0;
            _opened = false;
        }

        public void SetOpened(bool opened, bool animated = true)
        {
            _opened = opened;
            if (!animated)
                SetNewOffset(opened ? MaxOffset : 0);
            else
            {
                if (_animator != null)
                {
                    _animator.Cancel();
                    _animator = null;
                }

                _animator = ValueAnimator.OfInt(_contentOffsetX, opened ? MaxOffset : 0);
                _animator.SetInterpolator(_interpolator);
                _animator.SetDuration(Context.Resources.GetInteger(Android.Resource.Integer.ConfigMediumAnimTime));
                _animator.Update += (sender, e) => SetNewOffset((int)e.Animation.AnimatedValue);
                _animator.AnimationEnd += (sender, e) => { _animator.RemoveAllListeners(); _animator = null; };
                _animator.Start();
            }
        }

        void SetNewOffset(int newOffset)
        {
            var oldOffset = _contentOffsetX;
            _contentOffsetX = Math.Min(Math.Max(0, newOffset), MaxOffset);
            ContentView.OffsetLeftAndRight(_contentOffsetX - oldOffset);

            if (_opened && _contentOffsetX == 0)
                _opened = false;
            else if (!_opened && _contentOffsetX == MaxOffset)
                _opened = true;

            UpdateParallax();
            Invalidate();
        }

        void UpdateParallax()
        {
            var openness = ((float)(MaxOffset - _contentOffsetX)) / MaxOffset;
            MenuView.OffsetLeftAndRight((int)(-openness * MaxOffset * ParallaxSpeedRatio) - MenuView.Left);
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            // Only accept single touch
            if (ev.PointerCount != 1)
                return false;

            return CaptureMovementCheck(ev);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                CaptureMovementCheck(e);
                return true;
            }
            if (!_isTracking && !CaptureMovementCheck(e))
                return true;

            if (e.Action != MotionEventActions.Move || MoveDirectionTest(e))
                _velocityTracker.AddMovement(e);

            if (e.Action == MotionEventActions.Move)
            {
                var x = e.HistorySize > 0 ? e.GetHistoricalX(0) : e.GetX();
                var traveledDistance = (int)Math.Round(Math.Abs(x - (_startX)));
                SetNewOffset(_stateBeforeTracking ?
                              MaxOffset - Math.Min(MaxOffset, traveledDistance)
                              : Math.Max(0, traveledDistance));
            }
            else if (e.Action == MotionEventActions.Up
                     && _stateBeforeTracking == _opened)
            {
                _velocityTracker.ComputeCurrentVelocity(1000, _maxFlingVelocity);
                if (Math.Abs(_velocityTracker.XVelocity) > _minFlingVelocity)
                    SetOpened(!_opened);
                else if (!_opened && _contentOffsetX > MaxOffset / 2)
                    SetOpened(true);
                else if (_opened && _contentOffsetX < MaxOffset / 2)
                    SetOpened(false);
                else
                    SetOpened(_opened);

                _preTracking = _isTracking = false;
            }

            return true;
        }

        bool CaptureMovementCheck(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Down)
            {
                _startX = (int)ev.GetX();
                _startY = (int)ev.GetY();

                // Only work if the initial touch was in the start strip when the menu is closed
                // When the menu is opened, anywhere will do
                //if (!opened && (startX > Context.ToPixels(30)) )
                //    return false;

                _velocityTracker = VelocityTracker.Obtain();
                _velocityTracker.AddMovement(ev);
                _preTracking = true;
                _stateBeforeTracking = _opened;
                return false;
            }

            if (ev.Action == MotionEventActions.Up)
                _preTracking = _isTracking = false;

            if (!_preTracking)
                return false;

            _velocityTracker.AddMovement(ev);

            if (ev.Action == MotionEventActions.Move)
            {

                // Check we are going in the right direction, if not cancel the current gesture
                if (!MoveDirectionTest(ev))
                {
                    _preTracking = false;
                    return false;
                }

                // If the current gesture has not gone long enough don't intercept it just yet
                var distance = Math.Sqrt(Math.Pow(ev.GetX() - _startX, 2) + Math.Pow(ev.GetY() - _startY, 2));
                if (distance < _pagingTouchSlop)
                    return false;
            }

            _startX = (int)ev.GetX();
            _startY = (int)ev.GetY();
            _isTracking = true;

            return true;
        }

        // Check that movement is in a common vertical area and that we are going in the right direction
        bool MoveDirectionTest(MotionEvent e)
        {
            return (_stateBeforeTracking ? e.GetX() <= _startX : e.GetX() >= _startX)
                    && Math.Abs(e.GetY() - _startY) < _touchSlop;
        }

        protected override void DispatchDraw(Canvas canvas)
        {
            base.DispatchDraw(canvas);

            if (!_opened && !_isTracking && _animator == null) return;

            // Draw inset shadow on the menu
            canvas.Save();
            _shadowDrawable.SetBounds(0, 0, Context.ToPixels(8), Height);
            canvas.Translate(ContentView.Left - _shadowDrawable.Bounds.Width(), 0);
            _shadowDrawable.Draw(canvas);
            canvas.Restore();

            if (_contentOffsetX == 0) return;

            // Cover the area with a black overlay to display openess graphically
            var openness = ((float)(MaxOffset - _contentOffsetX)) / MaxOffset;
            _overlayPaint.Alpha = Math.Max(0, (int)(MaxOverlayAlpha * openness));
            if (_overlayPaint.Alpha > 0)
                canvas.DrawRect(0, 0, ContentView.Left, Height, _overlayPaint);
        }

        class SmoothInterpolator : Java.Lang.Object, ITimeInterpolator
        {
            public float GetInterpolation(float input)
            {
                return (float)Math.Pow(input - 1, 5) + 1;
            }
        }
    }

    static class DensityExtensions
    {
        static readonly DisplayMetrics DisplayMetrics = new DisplayMetrics();

        public static int ToPixels(this Context ctx, int dp)
        {
            var wm = ctx.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            wm.DefaultDisplay.GetMetrics(DisplayMetrics);

            var density = DisplayMetrics.Density;
            return (int)(dp * density + 0.5f);
        }
    }
}