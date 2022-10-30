using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        float bestTime = 0;

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed--;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10f).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();

                if ((tenthsOfSecondsElapsed / 10f) < bestTime || bestTime == 0)
                {
                    bestTime = (30 - tenthsOfSecondsElapsed) / 10f;
                }

                timeTextBlock.Text = $"Best time: {bestTime}s\n"
                    + (30 - tenthsOfSecondsElapsed) / 10f + " - Play again?";
            }
            else if (tenthsOfSecondsElapsed == 0)
            {
                timer.Stop();

                timeTextBlock.Text = "Time is over\nPlay again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🧜", "🧜",
                "💍", "💍",
                "🍺", "🍺",
                "🌈‍", "🌈‍",
                "🐱‍", "🐱‍",
                "💔", "💔",
                "🥑", "🥑",
                "🐸", "🐸",
                "👳‍", "👳‍",
                "‍🤦‍", "‍🤦‍",
                "‍✈", "‍✈",
                "‍👀", "‍👀",
                "‍🐗", "‍🐗",
                "‍🐭", "‍🐭",
                "‍🦄", "‍🦄",
                "‍🐶", "‍🐶",
            };

            Random random = new Random();

            while (animalEmoji.Count != 16)
            {
                int index = random.Next(animalEmoji.Count);
                if (index % 2 == 0)
                {
                    animalEmoji.RemoveAt(index);
                    animalEmoji.RemoveAt(index);
                }
                else
                {
                    animalEmoji.RemoveAt(index);
                    animalEmoji.RemoveAt(index - 1);
                }
            }

            foreach(TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    if(textBlock.Visibility == Visibility.Hidden)
                    {
                        textBlock.Visibility = Visibility.Visible;
                    }
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 30;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if(findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if(lastTextBlockClicked.Text == textBlock.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch= false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8 || tenthsOfSecondsElapsed == 0)
            {
                SetUpGame();
            }
        }
    }
}
