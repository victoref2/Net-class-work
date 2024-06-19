using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private void ipdigits1_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out int result) || result < 0 || result > 255)
            {
                e.Handled = true;
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

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ipdigits1.Text, out int part1) &&
                int.TryParse(ipdigits2.Text, out int part2) &&
                int.TryParse(ipdigits3.Text, out int part3) &&
                int.TryParse(ipdigits4.Text, out int part4) &&
                int.TryParse(netmask1.Text, out int nm1) &&
                int.TryParse(netmask2.Text, out int nm2) &&
                int.TryParse(netmask3.Text, out int nm3) &&
                int.TryParse(netmask4.Text, out int nm4))
            {
                var networkClass = GetNetworkClass(part1);
                var cidr = GetCidr(nm1, nm2, nm3, nm4);
                var wildcard = GetWildcard(nm1, nm2, nm3, nm4);
                var networkAddress = GetNetworkAddress(part1, part2, part3, part4, nm1, nm2, nm3, nm4);
                var broadcastAddress = GetBroadcastAddress(part1, part2, part3, part4, nm1, nm2, nm3, nm4);
                var usableHostRange = GetUsableHostRange(networkAddress, broadcastAddress);

                networkclass.Content = networkClass;
                networktype.Content = $"/{cidr}";
                wildcard1.Text = wildcard[0].ToString();
                wildcard2.Text = wildcard[1].ToString();
                wildcard3.Text = wildcard[2].ToString();
                wildcard4.Text = wildcard[3].ToString();
                NetworkAddress.Content = string.Join(".", networkAddress);
                BroadcastAddress.Content = string.Join(".", broadcastAddress);
                UsableHostRange.Content = $"{string.Join(".", usableHostRange.Item1)} - {string.Join(".", usableHostRange.Item2)}";
            }
            else
            {
                MessageBox.Show("Please enter valid IP address and netmask parts.");
            }
        }

        Dictionary<int, string> lookupTable = new Dictionary<int, string>
        {
            { 128, "A" },
            { 192, "B" },
            { 224, "C" },
            { 240, "D" },
            { 255, "E" }
        };

        private string GetNetworkClass(int firstOctet)
        {
            foreach (var entry in lookupTable)
            {
                if (firstOctet < entry.Key)
                {
                    return entry.Value;
                }
            }
            return "Fejl";
        }

        private int GetCidr(int nm1, int nm2, int nm3, int nm4)
        {
            var binaryNetmask = $"{Convert.ToString(nm1, 2).PadLeft(8, '0')}" +
                                $"{Convert.ToString(nm2, 2).PadLeft(8, '0')}" +
                                $"{Convert.ToString(nm3, 2).PadLeft(8, '0')}" +
                                $"{Convert.ToString(nm4, 2).PadLeft(8, '0')}";
            return binaryNetmask.Count(c => c == '1');
        }

        private int[] GetWildcard(int nm1, int nm2, int nm3, int nm4)
        {
            return new int[]
            {
        255 - nm1,
        255 - nm2,
        255 - nm3,
        255 - nm4
            };
        }

        private int[] GetNetworkAddress(int ip1, int ip2, int ip3, int ip4, int nm1, int nm2, int nm3, int nm4)
        {
            return new int[]
            {
        ip1 & nm1,
        ip2 & nm2,
        ip3 & nm3,
        ip4 & nm4
            };
        }

        private int[] GetBroadcastAddress(int ip1, int ip2, int ip3, int ip4, int nm1, int nm2, int nm3, int nm4)
        {
            return new int[]
            {
        ip1 | (255 - nm1),
        ip2 | (255 - nm2),
        ip3 | (255 - nm3),
        ip4 | (255 - nm4)
            };
        }

        private Tuple<int[], int[]> GetUsableHostRange(int[] networkAddress, int[] broadcastAddress)
        {
            var startAddress = (int[])networkAddress.Clone();
            var endAddress = (int[])broadcastAddress.Clone();

            startAddress[3] += 1; // Network address + 1
            endAddress[3] -= 1;   // Broadcast address - 1

            return new Tuple<int[], int[]>(startAddress, endAddress);
        }

    }
}
