using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Word_Generator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            SetUpSliderForMouse();
            A.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            A.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void SearchWords(object sender, RoutedEventArgs e)
        {
            SearchResult.Text = Core.GetWords();
            FoundWordsCount.Text = Core.GetCount();
        }
        
        private void UpdateSliderNumber(object sender, MouseEventArgs e)
        {
            Core.WordLength = (int) Slider.Value;
            NumberOfLetters.Text = Slider.Value.ToString();
            CheckSelect();
        }

        private void AddLetter(object sender, RoutedEventArgs e)
        {
            Core.AddLetter(sender as Button);
            CheckSelect();
        }

        private void RemoveLetter(object sender, RoutedEventArgs e)
        {
            Core.RemoveLetter(sender as Button);
            CheckSelect();
        }

        private void ResetCharsNeeded(object sender, RoutedEventArgs e)
        {
            Core.ResetCharsRequired();
            CheckSelect();
        }

        private void ResetCharsNotNeeded(object sender, RoutedEventArgs e)
        {
            Core.ResetCharsNotRequired();
            CheckSelect();
        }

        private void CheckSelect()
        {
            var instructions = Core.CheckParams();
            ErrorForNeededLetters.Text = instructions[0];
            ErrorForNotNeededLetters.Text = instructions[1];
            ErrorIfSeceltedSame.Text = instructions[2];
        }

        private void SetUpSliderForMouse()
        {
            Slider.ApplyTemplate();
            Thumb thumb = (Slider.Template.FindName("PART_Track", Slider) as Track).Thumb;
            thumb.MouseEnter += new MouseEventHandler(ThumbMouseEnter);
        }
        
        private void ThumbMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.MouseDevice.Captured == null)
            {
                MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;
                (sender as Thumb).RaiseEvent(args);
            }
        }

        private void CopyAlphabet(object sender, RoutedEventArgs e)
        {
            Core.GetAlphabetString();
        }

        private void CopyNumberRow(object sender, RoutedEventArgs e)
        {
            Core.GetNumberRow(Convert.ToInt32(NumberRow.Text));
        }

        private void CopyTrafficlights(object sender, RoutedEventArgs e)
        {
            int redLight = Convert.ToInt32(RedCircle.Text);
            int yelloweLight = Convert.ToInt32(YellowCircle.Text);
            int greenLight = Convert.ToInt32(GreenCircle.Text);
            Core.GetTrafficlightString(redLight, yelloweLight, greenLight);
        }

        private void ResetTrafficlights(object sender, RoutedEventArgs e)
        {
            RedCircle.SelectedIndex = 0;
            YellowCircle.SelectedIndex = 0;
            GreenCircle.SelectedIndex = 0;
        }

        private void CopyAmounts(object sender, RoutedEventArgs e)
        {
            int one = Convert.ToInt32(OnePounds.Text);
            int two = Convert.ToInt32(TwoPounds.Text);
            int three = Convert.ToInt32(ThreePounds.Text);
            int four = Convert.ToInt32(FourPounds.Text);
            int five = Convert.ToInt32(FivePounds.Text);
            int ten = Convert.ToInt32(TenPounds.Text);
            int fifteen = Convert.ToInt32(FifteenPounds.Text);
            int twenty = Convert.ToInt32(TwentyPounds.Text);
            int twentyFive = Convert.ToInt32(TwentyFivePounds.Text);
            int thirty = Convert.ToInt32(ThirtyPounds.Text);
            int thirtyFive = Convert.ToInt32(ThirtyFivePounds.Text);

            Core.GetAmountsString(one, two, three, four, five, ten, fifteen, twenty, twentyFive, thirty, thirtyFive);
        }

        private void ResetSums(object sender, RoutedEventArgs e)
        {
            FivePounds.SelectedIndex = 0;
            TenPounds.SelectedIndex = 0;
            FifteenPounds.SelectedIndex = 0;
            TwentyPounds.SelectedIndex = 0;
            TwentyFivePounds.SelectedIndex = 0;
            ThirtyPounds.SelectedIndex = 0;
            ThirtyFivePounds.SelectedIndex = 0;
        }
        
        private void AddAmount(object sender, RoutedEventArgs e)
        {
            Core.AddAmountToChance(sender as Button);
        }

        private void CopyChances(object sender, RoutedEventArgs e)
        {
            Core.GetChanceString(Convert.ToInt32(ChanceAmount.Text));
        }

        private void ResetChances(object sender, RoutedEventArgs e)
        {
            ChanceAmount.SelectedIndex = 0;
            Core.ResetChances();
        }
    }
}
