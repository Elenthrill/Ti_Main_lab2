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
using System.IO;
using StreamEncrypt.BackEnd;

namespace StreamEncrypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void tbox_showLen(object sender, TextChangedEventArgs e)
        {
            TBox_RegLen.Text = TBox_key.Text.Length.ToString();
        }
        private void Tbox_inputblock(object sender, TextCompositionEventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length <= 50)
            if (!(e.Text == "1" || e.Text == "0"))
            {
                e.Handled = true;
            }
        }

        private void btn_save_click(Object sender, EventArgs e)
        {
            if(TBox_cyphrotext.Text.Length == 0)
            {
                MessageBox.Show("Нечего сохранять");
                return;
            }
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                byte[] result = new byte[Encryption.ciphertext.Length / 8];
                for (int i = 0; i < result.Length; i++)
                {
                    byte oneByte = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        oneByte |= (byte)(Encryption.ciphertext[i * 8 + j] << j);
                    }
                    result[i] = oneByte;
                }
                string filePath = saveFileDialog.FileName;

                File.WriteAllBytes(filePath, result);
            }
        }
        private void btn_open_clik(Object sender, EventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                byte[] bytes = File.ReadAllBytes(filePath);
                Encryption.plaintext = new byte[bytes.Length * 8];
                Encryption.ciphertext = new byte[bytes.Length * 8];
                for (int i = 0; i < bytes.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Encryption.plaintext[i * 8 + j] = (byte)((bytes[i] >> j) & 0x1);
                    }
                }
                ShowText(TBox_plaintext,Encryption.plaintext);
                // Использование содержимого файла
            }
        }
        private void ShowText(TextBox sender,byte[] bits)
        {
            sender.Text = bits.Length > 30 * 8 ? "Первые 15 байт: " + BackEnd.Convert.BitsToString(bits, 0, 15 * 8) + Environment.NewLine + "Последние 15 байт: " + BackEnd.Convert.BitsToString(bits, bits.Length - 15 * 8, 15 * 8) :
                                                         BackEnd.Convert.BitsToString(bits, 0, bits.Length);
        }

        private void btn_Crypt_Click(object sender, RoutedEventArgs e)
        {
            if (TBox_plaintext.Text.Length == 0 || TBox_RegLen.Text != "31")
            {
                MessageBox.Show("Введенные данные некорректны, попробуйте снова");
                return;
            }
            byte[] keyBytes = new byte[KeyGen.polinom[0]];
            int i = 0;
            for (int j = 0; j < TBox_key.Text.Length; j++)
            {
                if (TBox_key.Text[j] == '0' || TBox_key.Text[j] == '1')
                {
                    keyBytes[i] = (byte)(TBox_key.Text[j] - '0');
                    i++;
                }
            }
            var newkey = BackEnd.KeyGen.Generate(keyBytes, BackEnd.Encryption.plaintext.Length);
            Encryption.Encrypt(newkey);
            ShowGeneratedKey(newkey);
            ShowText(TBox_cyphrotext, Encryption.ciphertext);
        }
        private void ShowGeneratedKey(byte[] bits)
        {
            TBox_generatedkey.Text = bits.Length > 30 * 8 ? "Первые 15 байт: " + BackEnd.Convert.BitsToString(bits, 0, 15 * 8) + Environment.NewLine + "Последние 15 байт: " + BackEnd.Convert.BitsToString(bits, bits.Length - 15 * 8, 15 * 8) :
                                                              BackEnd.Convert.BitsToString(bits, 0, bits.Length);
        }

        private void btn_open_clik(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                byte[] bytes = File.ReadAllBytes(filePath);
                Encryption.plaintext = new byte[bytes.Length * 8];
                Encryption.ciphertext = new byte[bytes.Length * 8];
                TBox_generatedkey.Text = "";
                TBox_cyphrotext.Text = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Encryption.plaintext[i * 8 + j] = (byte)((bytes[i] >> j) & 0x1);
                    }
                }
                ShowText(TBox_plaintext, Encryption.plaintext);
                // Использование содержимого файла
            }
        }
    }

}
