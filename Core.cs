using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Word_Generator
{
    public static class Core
    {
        public static int WordLength { get; set; } = 5;
        private static List<string> wordsTotal = LoadDictionary();
        private static List<string> wordsFound = new List<string>();

        private static Dictionary<char, bool> charsIncludedInSearch = GetAlphabet();
        private static Dictionary<char, bool> charsExcludedFromSearch = GetAlphabet();
        private static string charsNeededToPresent;
        private static string charsNotNeededToPresent;

        private static int buttonsPressedInInclude;
        private static int buttonsPressedInExclude;
        private static List<Button> buttonsActiveInInclude = new List<Button>();
        private static List<Button> buttonsActiveInExclude = new List<Button>();

        private static string redEmoji = char.ConvertFromUtf32(0x0001F534).ToString();
        private static string yellowEmoji = char.ConvertFromUtf32(0x0001F7E1).ToString();
        private static string greenEmoji = char.ConvertFromUtf32(0x0001F7E2).ToString();

        private static int chanceAmounts = 8;
        private static List<Button> chanceButtons = new List<Button>();
        private static bool[] amounts = new bool[chanceAmounts];

        public static string GetWords()
        {
            return GetTextToDisplay();
        }

        public static string GetCount()
        {
            return "Найдено: " + wordsFound.Count;
        }

        private static string GetTextToDisplay()
        {
            FindWords();

            StringBuilder stringToDisplay = new StringBuilder();

            foreach (string word in wordsFound)
                stringToDisplay.Append(word + "\n");

            return stringToDisplay.ToString();
        }

        private static void FindWords()
        {
            wordsFound.Clear();
            GetChars();

            char[] toCheck;

            foreach (string word in wordsTotal)
            { 
                toCheck = word.Distinct().ToArray();

                if (word.Length == toCheck.Length &&
                    word.Length == WordLength &&
                    IsCharsPresent(word, charsNeededToPresent) &&
                    IsCharsNotPresent(word, charsNotNeededToPresent))

                    wordsFound.Add(word);
            }
        }

        private static void GetChars()
        {
            StringBuilder charsInclude = new StringBuilder();

            foreach (KeyValuePair<char, bool> letter in charsIncludedInSearch)
                if (letter.Value)
                    charsInclude.Append(letter.Key);

            charsNeededToPresent = charsInclude.ToString();

            StringBuilder charsExclude = new StringBuilder();

            foreach (KeyValuePair<char, bool> letter in charsExcludedFromSearch)
                if (letter.Value)
                    charsExclude.Append(letter.Key);

            charsNotNeededToPresent = charsExclude.ToString();
        }

        private static bool IsCharsPresent(string word, string letters)
        {
            foreach (char c in letters)
                if (!word.Contains(c))
                    return false;
            
            return true;
        }

        private static bool IsCharsNotPresent(string word, string letters)
        {
            foreach (char c in letters)
                if (word.Contains(c))
                    return false;

            return true;
        }

        public static void AddLetter(Button button)
        {
            buttonsActiveInInclude.Add(button);
            char letter = button.Name[0];

            if (button.Background == Brushes.White)
            {
                buttonsPressedInInclude++;
                charsIncludedInSearch[letter] = true;
                button.Background = Brushes.LightGreen;
            }
            else
            {
                buttonsPressedInInclude--;
                charsIncludedInSearch[letter] = false;
                button.Background = Brushes.White;
            }
        }

        public static void RemoveLetter(Button button)
        {
            buttonsActiveInExclude.Add(button);
            char letter = button.Name.ToLower()[0];

            if (button.Background == Brushes.White)
            {
                buttonsPressedInExclude++;
                charsExcludedFromSearch[letter] = true;
                button.Background = Brushes.Pink;
            }
            else
            {
                buttonsPressedInExclude--;
                charsExcludedFromSearch[letter] = false;
                button.Background = Brushes.White;
            }
        }

        public static string[] CheckParams()
        {
            string include = "";
            string exclude = "";
            string same = "";

            if (buttonsPressedInInclude > WordLength)
                include = "Выбрано больше букв, чем длина слова";

            if (33 - buttonsPressedInExclude < WordLength)
                exclude = "Оставлено меньше букв, чем длина слова";

            if (CheckIfSelectedSame())
                same = "Выбраны одинаковые буквы";

            return new string[] { include, exclude, same };
        }

        private static bool CheckIfSelectedSame()
        {
            string chars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

            for (int i = 0; i < chars.Length; i++)
            {
                if (charsIncludedInSearch[chars[i]] & charsExcludedFromSearch[chars[i]]) 
                    return true;
            }

            return false;
        }

        public static void ResetCharsRequired()
        {
            ClearButtons(buttonsActiveInInclude);
            ClearDictionary(charsIncludedInSearch);
            buttonsActiveInInclude.Clear();
            buttonsPressedInInclude = 0;
        }

        public static void ResetCharsNotRequired()
        {
            ClearButtons(buttonsActiveInExclude);
            ClearDictionary(charsExcludedFromSearch);
            buttonsActiveInExclude.Clear();
            buttonsPressedInExclude = 0;
        }

        private static void ClearDictionary(Dictionary<char, bool> letters)
        {
            char[] chars = letters.Keys.ToArray();

            foreach (char c in chars)
                letters[c] = false;
        }

        private static void ClearButtons(List<Button> buttonsNeeded)
        {
            foreach (Button button in buttonsNeeded)
                button.Background = Brushes.White;
        }

        private static List<string> LoadDictionary()
        {
            var file = new StreamReader("Words.txt");
            var dictionary = new List<string>();
            string line;
            
            while ((line = file.ReadLine()) != null)
            {
                dictionary.Add(line.ToLower());
            }

            return dictionary;
        }

        private static Dictionary<char, bool> GetAlphabet()
        {
            var chars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            var alphabet = new Dictionary<char, bool>();

            foreach (char c in chars)
                alphabet.Add(c, false);

            return alphabet;
        }

        public static void GetAlphabetString()
        {
            var alphabet = "А\nБ\nВ\nГ\nД\nЕ\nЁ\nЖ\nЗ\nИ\nЙ\n" +
                           "К\nЛ\nМ\nН\nО\nП\nР\nС\nТ\nУ\nФ\n" +
                           "Х\nЦ\nЧ\nШ\nЩ\nЪ\nЫ\nЬ\nЭ\nЮ\nЯ";

            Clipboard.SetText(alphabet);
        }

        public static void GetNumberRow(int count)
        {
            var numberRow = "";

            for (int i = 0; i < count; i++)
                numberRow += (i + 1) + "\n";

            numberRow = numberRow.TrimEnd('\n');

            Clipboard.SetText(numberRow);
        }

        public static void GetTrafficlightString(int red, int yellow, int green)
        {
            var trafficLights = "";

            for (int i = 0; i < red; i++)
                trafficLights += redEmoji + "\n";

            for (int i = 0; i < yellow; i++)
                trafficLights += yellowEmoji + "\n";

            for (int i = 0; i < green; i++)
                trafficLights += greenEmoji + "\n";

            trafficLights = trafficLights.TrimEnd('\n');

            Clipboard.SetText(trafficLights);
        }

        public static void GetAmountsString(int one, int two, int three, int four, int five, int ten, int fifteen, int twenty, int twentyFive, int thirty, int thirtyFive)
        {
            var amounts = "";

            for (int i = 0; i < one; i++)
                amounts += "£1" + "\n";

            for (int i = 0; i < two; i++)
                amounts += "£2" + "\n";

            for (int i = 0; i < three; i++)
                amounts += "£3" + "\n";

            for (int i = 0; i < four; i++)
                amounts += "£4" + "\n";

            for (int i = 0; i < five; i++)
                amounts += "£5" + "\n";

            for (int i = 0; i < ten; i++)
                amounts += "£10" + "\n";

            for (int i = 0; i < fifteen; i++)
                amounts += "£15" + "\n";

            for (int i = 0; i < twenty; i++)
                amounts += "£20" + "\n";

            for (int i = 0; i < twentyFive; i++)
                amounts += "£25" + "\n";

            for (int i = 0; i < thirty; i++)
                amounts += "£30" + "\n";

            for (int i = 0; i < thirtyFive; i++)
                amounts += "£35" + "\n";

            amounts = amounts.TrimEnd('\n');

            Clipboard.SetText(amounts);
        }

        public static void AddAmountToChance(Button button)
        {
            int i = button.Name[0] - 65;

            if (button.Background == Brushes.White)
            {
                amounts[i] = true;
                chanceButtons.Add(button);
                button.Background = Brushes.LightGreen;
            }
            else
            {
                amounts[i] = false;
                button.Background = Brushes.White;
            }
        }

        public static void GetChanceString(int amount)
        {
            var chances = "";

            for (int i = 0; i < amount; i++)
            {
                if (amounts[0]) chances += "£0" + "\n";
                if (amounts[1]) chances += "£5" + "\n";
                if (amounts[2]) chances += "£10" + "\n";
                if (amounts[3]) chances += "£15" + "\n";
                if (amounts[4]) chances += "£20" + "\n";
                if (amounts[5]) chances += "£25" + "\n";
                if (amounts[6]) chances += "£30" + "\n";
                if (amounts[7]) chances += "£35" + "\n";
                chances += "ШАНС" + "\n";
            }

            chances = chances.TrimEnd('\n');

            Clipboard.SetText(chances);
        }

        public static void ResetChances()
        {
            for (int i = 0; i < chanceAmounts; i++)
                amounts[i] = false;

            ClearButtons(chanceButtons);
        }
    }
}