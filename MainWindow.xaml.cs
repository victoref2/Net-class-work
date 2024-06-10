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

namespace Number_system_conveter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int Universal = 0;

        public void Base_10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Base_10.IsFocused)
            {
                if (int.TryParse(Base_10.Text, out int base10Value))
                {
                    Universal = base10Value;
                    UpdateTextBoxes();
                }
                else
                {
                    ClearOtherTextBoxes();
                }
            }
        }

        public void Base_2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Base_2.IsFocused)
            {
                if (TryParseBase(Base_2.Text, 2, out int base2Value))
                {
                    Universal = base2Value;
                    UpdateTextBoxes();
                }
                else
                {
                    ClearOtherTextBoxes();
                }
            }
        }

        public void Base_16_hexadecimal_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Base_16_hexadecimal.IsFocused)
            {
                if (TryParseBase(Base_16_hexadecimal.Text, 16, out int base16Value))
                {
                    Universal = base16Value;
                    UpdateTextBoxes();
                }
                else
                {
                    ClearOtherTextBoxes();
                }
            }
        }

        public void Base_8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Base_8.IsFocused)
            {
                if (TryParseBase(Base_8.Text, 8, out int base8Value))
                {
                    Universal = base8Value;
                    UpdateTextBoxes();
                }
                else
                {
                    ClearOtherTextBoxes();
                }
            }
        }
        public void Base_32_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Base_32.IsFocused)
            {
                if (TryParseBase(Base_32.Text, 32, out int base32Value))
                {
                    Universal = base32Value;
                    UpdateTextBoxes();
                }
                else
                {
                    ClearOtherTextBoxes();
                }
            }
        }

        private bool TryParseBase(string input, int fromBase, out int result)
        {
            try
            {
                result = Convert.ToInt32(input, fromBase);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        private void UpdateTextBoxes()
        {
            Base_10.Text = Universal.ToString();
            Base_2.Text = Convert.ToString(Universal, 2);
            Base_8.Text = Convert.ToString(Universal, 8);
            Base_16_hexadecimal.Text = Convert.ToString(Universal, 16).ToUpper();
            Base_32.Text = ConvertToBase32(Universal);
        }

        private void ClearOtherTextBoxes()
        {
            Base_2.Text = string.Empty;
            Base_10.Text = string.Empty;
            Base_16_hexadecimal.Text = string.Empty;
            Base_8.Text = string.Empty;
            Base_32.Text = string.Empty;
        }

        private string ConvertToBase32(int value)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
            string result = string.Empty;
            int targetBase = 32;

            do
            {
                result = chars[value % targetBase] + result;
                value /= targetBase;
            } while (value > 0);

            return result;
        }

        private int ConvertFromBase32(string input)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
            int result = 0;
            int base32 = 32;

            foreach (char c in input)
            {
                result = result * base32 + chars.IndexOf(c);
            }

            return result;
        }
    }
}
