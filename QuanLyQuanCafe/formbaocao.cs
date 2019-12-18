using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class formbaocao : Form


    
            
    {



         public formbaocao()
        {
            InitializeComponent();

         }

        public void ShowBill2(int id)
        {

            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
              //  lsvhd.Items.Add(lsvItem);

            }

            CultureInfo culture = new CultureInfo("vi-VN");

            //Thread.CurrentThread.CurrentCulture = culture;

           // txttongtien.Text = totalPrice.ToString("c", culture);

        }




        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Formbaocao_Load(object sender, EventArgs e)
        {

        }

        private void Lsvhd_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
