using System.Windows;
using System.Windows.Input;

namespace QTrack.Behaviors
{
    public static class ConditionalModifierEnableBehavior
    {
        public readonly static DependencyProperty IsConditionMetProperty =
            DependencyProperty.RegisterAttached(
                "IsConditionMet",
                typeof(bool),
                typeof(ConditionalModifierEnableBehavior),
                new PropertyMetadata(true, OnPropertyChanged));

        public readonly static DependencyProperty ModifierKeyProperty =
            DependencyProperty.RegisterAttached(
                "ModifierKey",
                typeof(ModifierKeys),
                typeof(ConditionalModifierEnableBehavior),
                new PropertyMetadata(ModifierKeys.Shift, OnPropertyChanged));

        public static bool GetIsConditionMet(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsConditionMetProperty);
        }

        public static void SetIsConditionMet(DependencyObject obj, bool value)
        {
            obj.SetValue(IsConditionMetProperty, value);
        }

        public static ModifierKeys GetModifierKey(DependencyObject obj)
        {
            return (ModifierKeys)obj.GetValue(ModifierKeyProperty);
        }

        public static void SetModifierKey(DependencyObject obj, ModifierKeys value)
        {
            obj.SetValue(ModifierKeyProperty, value);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                element.Loaded -= OnElementLoaded;
                element.Loaded += OnElementLoaded;
                element.Unloaded -= OnElementUnloaded;
                element.Unloaded += OnElementUnloaded;

                UpdateElementState(element);
            }
        }

        private static void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                var window = Window.GetWindow(element);
                if (window != null)
                {
                    window.PreviewKeyDown -= OnWindowKeyEvent;
                    window.PreviewKeyDown += OnWindowKeyEvent;
                    window.PreviewKeyUp -= OnWindowKeyEvent;
                    window.PreviewKeyUp += OnWindowKeyEvent;
                }

                UpdateElementState(element);
            }
        }

        private static void OnElementUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                var window = Window.GetWindow(element);
                if (window != null)
                {
                    window.PreviewKeyDown -= OnWindowKeyEvent;
                    window.PreviewKeyUp -= OnWindowKeyEvent;
                }
            }
        }

        private static void OnWindowKeyEvent(object sender, KeyEventArgs e)
        {
            if (sender is Window window)
            {
                // ウィンドウ内の該当ビヘイビアが設定されている要素の状態を再評価
                UpdateChildElements(window);
            }
        }

        private static void UpdateChildElements(DependencyObject parent)
        {
            var childCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childCount; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement element)
                {
                    UpdateElementState(element);
                }

                UpdateChildElements(child);
            }
        }

        private static void UpdateElementState(FrameworkElement element)
        {
            var isConditionMet = GetIsConditionMet(element);
            var requiredModifier = GetModifierKey(element);

            var isModifierPressed = (Keyboard.Modifiers & requiredModifier) == requiredModifier;

            // 条件を満たしている、または指定修飾キーが押されている場合に有効化
            element.IsEnabled = isConditionMet || isModifierPressed;
        }
    }
}