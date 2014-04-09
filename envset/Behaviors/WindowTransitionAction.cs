using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Interactivity;

using Livet.Messaging;
using Livet.Behaviors.Messaging;
using Livet.Behaviors;


namespace envset.Behaviors
{
    public class WindowTransitionAction : TriggerAction<FrameworkElement>
    {
        public enum TransitionMode
        {
            Show,
            ShowDialog,
            ShowAndCloseSourceWindow,
        }


        #region SourceWindow
        public Window SourceWindow
        {
            get { return (Window)GetValue(SourceWindowProperty); }
            set { SetValue(SourceWindowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceWindowProperty =
            DependencyProperty.Register("SourceWindow", typeof(Window), typeof(WindowTransitionAction), new PropertyMetadata(null));
        #endregion

        #region DestinationWindow
        public Type DestinationWindow
        {
            get { return (Type)GetValue(DestinationWindowProperty); }
            set { SetValue(DestinationWindowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DestinationWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DestinationWindowProperty =
            DependencyProperty.Register("DestinationWindow", typeof(Type), typeof(WindowTransitionAction), new PropertyMetadata(null));
        #endregion

        #region Mode
        public TransitionMode Mode
        {
            get { return (TransitionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TransitionType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(TransitionMode), typeof(WindowTransitionAction), new PropertyMetadata(TransitionMode.Show));
        #endregion

        #region DestinationWindowTag
        public object DestinationWindowTag
        {
            get { return (object)GetValue(DestinationWindowTagProperty); }
            set { SetValue(DestinationWindowTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DestinationWindowTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DestinationWindowTagProperty =
            DependencyProperty.Register("DestinationWindowTag", typeof(object), typeof(WindowTransitionAction), new PropertyMetadata(null));
        #endregion

        #region OnClosedSourceWindowTarget
        public object OnClosedSourceWindowTarget
        {
            get { return (object)GetValue(OnClosedSourceWindowTargetProperty); }
            set { SetValue(OnClosedSourceWindowTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnClosedSourceWindowTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnClosedSourceWindowTargetProperty =
            DependencyProperty.Register("OnClosedSourceWindowTarget", typeof(object), typeof(WindowTransitionAction), new PropertyMetadata(null));
        #endregion

        #region OnClosedSourceWindow
        public string OnClosedSourceWindow
        {
            get { return (string)GetValue(OnClosedSourceWindowProperty); }
            set { SetValue(OnClosedSourceWindowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnClosedSourceWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnClosedSourceWindowProperty =
            DependencyProperty.Register("OnClosedSourceWindow", typeof(string), typeof(WindowTransitionAction), new PropertyMetadata(null));
        #endregion



        protected override void Invoke(object parameter)
        {
            if (SourceWindow == null)
            {
                var candidate = AssociatedObject;
                while (candidate != null)
                {
                    if (candidate is Window)
                    {
                        SourceWindow = candidate as Window;
                        break;
                    }
                    candidate = candidate.Parent as FrameworkElement;
                }
            }

            if (DestinationWindow == null)
            {
                throw new ArgumentException("DestinationWindow is null");
            }
            var window = Activator.CreateInstance(DestinationWindow) as Window;
            if (window == null)
            {
                throw new ArgumentException("Destination Window cannot be type Window");
            }
            window.Owner = SourceWindow;
            window.Tag = DestinationWindowTag;

            if (OnClosedSourceWindowTarget != null && OnClosedSourceWindow != null)
            {
                // コールバック実行時に参照が消えないようにアーカイブしとく
                // ていうかなんで参照消えるの？？
                var target = OnClosedSourceWindowTarget;
                var name = OnClosedSourceWindow;
                window.Closed += (sender, e) => { new MethodBinder().Invoke(target, name); };
            }

            switch(Mode)
            {
                case TransitionMode.Show:
                    window.Show();
                    break;

                case TransitionMode.ShowDialog:
                    window.ShowDialog();
                    break;

                case TransitionMode.ShowAndCloseSourceWindow:
                    window.Show();
                    if (SourceWindow != null)
                    {
                        SourceWindow.Close();
                    }
                    break;

                default:
                    throw new Exception("Unexpected Error: Mode is INVALID");
            }
        }
    }
}
