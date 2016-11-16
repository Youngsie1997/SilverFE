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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Npgsql;
using System.Globalization;

namespace SilverFE
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input :MetroWindow
    {
        public Input()
        {
            InitializeComponent();
        }
        NpgsqlConnection conn = new NpgsqlConnection("Server=murky.freeborough.com;Port=5432;User Id=connor;Password=poop123;Database=silver");

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Insert();
        }


        #region Connection Stuff
        public void Openconn()
        {
            try
            {
                conn.Open();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.ToString());
            }
        }

        public void Closeconn()
        {
            try
            {
                conn.Close();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.ToString());
            }
        }

        #endregion


        private void Insert()
        {
            try
            {
                Decimal Buy = Decimal.Parse(tbBuy.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                Decimal Sell = Decimal.Parse(tbSell.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                Decimal Profit = Sell - Buy;

                this.Openconn();
                string sql = "INSERT INTO " + cbTable.Text.ToString() + "(Type,Year,Qty,Mint,Buy,Sell,Profit) VALUES('" + tbType.Text.ToString() + "','" + tbYear.Text.ToString() + "','" + tbQty.Text.ToString() + "','" + tbMint.Text.ToString() + "','"+Buy+"','"+Sell+"','"+Profit+"')";
                NpgsqlCommand InsertCommand = new NpgsqlCommand(sql, conn);
                int Success = InsertCommand.ExecuteNonQuery();
                
                if(Success >= 1)
                {
                    MessageBox.Show("Sucess");
                }
                else
                {
                    MessageBox.Show("Something went wrong :(");
                }
                Closeconn();
            }
            catch(Exception Error)
            {
                MessageBox.Show(Error.ToString(), "Insert Failed");
            }
           

            
        }
    }








}
