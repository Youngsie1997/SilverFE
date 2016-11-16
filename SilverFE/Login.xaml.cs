using System;
using System.Windows;
using System.Windows.Input;
using System.Data;
using MahApps.Metro.Controls;
using Npgsql;


namespace SilverFE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        public Login()
        {
            InitializeComponent();
        }

      

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            login();
        }


        
        private void login()
        {
            if(tbUserName.Text == "" || tbPassword.Password == "")
            {
                MessageBox.Show("Please provide Username and Password", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=murky.freeborough.com;Port=5432;User Id=connor;Password=poop123;Database=silver;");
                conn.Open();
                string sql = "SELECT * FROM Login WHERE UserName='" + tbUserName.Text + "' and Password='" + tbPassword.Password + "'";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                ds.Reset();
                da.Fill(ds);
                int count = ds.Tables[0].Rows.Count;
                conn.Close();
                if(count == 1 && rbView.IsChecked == true) 
                {
                    MessageBox.Show("Login Successful", "Login", MessageBoxButton.OK, MessageBoxImage.None);
                    string sql2 = "UPDATE login SET last_login='now' WHERE ID='" + ds.Tables[0].Rows[0][0] + "'";
                    NpgsqlCommand logindate = new NpgsqlCommand(sql2, conn);
                    conn.Open();
                    logindate.ExecuteNonQuery();
                    conn.Close();
                    Main main = new Main();
                    main.Show();
                    this.Close();
                    
                }
                else if(count == 0)
                {
                    MessageBox.Show("Login Failed", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (count == 1 && rbInput.IsChecked == true)
                {
                    MessageBox.Show("Login Successful", "Login", MessageBoxButton.OK, MessageBoxImage.None);
                    string sql2 = "UPDATE login SET last_login='now' WHERE ID='" + ds.Tables[0].Rows[0][0] + "'";
                    NpgsqlCommand logindate = new NpgsqlCommand(sql2, conn);
                    conn.Open();
                    logindate.ExecuteNonQuery();
                    Input input = new Input();
                    input.Show();
                    conn.Close();
                    this.Close();
                }

            }
            catch(Exception Error)
            {
                MessageBox.Show(Error.ToString());
            }
           
            
           
            
            
        }


























        #region Misc

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tbUserName.Focus();
            rbView.IsChecked = true;
            tbUserName.MaxLength = 20;
            tbPassword.MaxLength = 20;
        }



        private void tbUserName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                e.Handled = true;
            }

            if(e.Key == Key.Enter)
            {
                tbPassword.Focus();
            }
        }

        private void tbPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                e.Handled = true;
            }

            if(e.Key == Key.Return)
            {
                login();
            }
        }
        #endregion

   





    }
}
