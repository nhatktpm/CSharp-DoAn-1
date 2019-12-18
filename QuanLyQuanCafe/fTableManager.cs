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
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        public fTableManager(Account acc)
        {
            InitializeComponent();
           
            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwitchTable);
          
        }

        #region Method
        

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                // btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
                btn.FlatAppearance.BorderColor = System.Drawing.Color.Red;

                //btn.Image = Image.FromFile(".Assets/vicons8 - restaurant - table - 50png");

                btn.Click += Btn_Click;
                btn.Tag = item;
                
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.MediumOrchid;
                        break;
                }

                flpTable.Controls.Add(btn);
                
            }
            
        }





        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
                
                
            }

            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;


            double finalTotalPrice = (totalPrice - (totalPrice / 100) * discount);


            CultureInfo culture = new CultureInfo("vi-VN");

            //Thread.CurrentThread.CurrentCulture = culture;

            txbTotalPrice.Text = finalTotalPrice.ToString("c", culture);

        }


        void ShowBill2(int id)
        {
            lsvhd.Items.Clear();
            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem2 = new ListViewItem(item.FoodName.ToString());
                lsvItem2.SubItems.Add(item.Count.ToString());
                lsvItem2.SubItems.Add(item.Price.ToString());
                lsvItem2.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvhd.Items.Add(lsvItem2);
                

            }

            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;

            
            double finalTotalPrice = (totalPrice - (totalPrice / 100) * discount) ;
            laban.Text = table.Name;
            tenban.Text = table.Name;
            txttongtien.Text = finalTotalPrice.ToString();
            decimal abc = nmDisCount.Value;
            txtgiamgia.Text=Convert.ToString(abc) ;
            
        }







        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion


        #region Events

        void Btn_Click(object sender, EventArgs e)
        {
            
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
            ShowBill2(tableID);
        }


        

        private void Button1_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;


            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);



            formbaocao f = new formbaocao();
            f.Show();
            

        }


        private void ĐăngXuấtToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ThôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }


        private void AdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog();
        }
        

        private void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void CbCategory_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);

        }

        #endregion

        

        private void FlpTable_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void DomainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void BtnCheckOut_Click_1(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = (totalPrice  - (totalPrice / 100) * discount )* 1000;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }


        private void BtnSwitchTable_Click_1(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;

            int id2 = (cbSwitchTable.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }
        }

        private void TxbTotalPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnDiscount_Click(object sender, EventArgs e)
        {

        }

        private void BtnAddFood_Click_1(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Chọn bàn hả chọn món nha");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);
            LoadTable();
        }

        private void TxbTotalPrice_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Btninhoadon_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;

            double totalPrice = Convert.ToDouble(txttongtien.Text.Split(',')[0]);
            double finalTotalPrice = (totalPrice - (totalPrice / 100) * discount) * 1000;
            ShowBill2(table.ID);

            LoadTable();
           
            
        }

        private void Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FTableManager_Load(object sender, EventArgs e)
        {

        }
        #region tính tiền thừa
        private void Label10_Click(object sender, EventArgs e)
        {
            double z = 100000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }

        private void Label14_Click(object sender, EventArgs e)
        {
            double z = 150000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }

        private void Label12_Click(object sender, EventArgs e)
        {
            double z = 250000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }

        private void Label13_Click(object sender, EventArgs e)
        {
            double z = 200000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }

        private void Label11_Click(object sender, EventArgs e)
        {
            double z = 300000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }

        private void Label15_Click(object sender, EventArgs e)
        {
            double z = 350000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }

        private void Label16_Click(object sender, EventArgs e)
        {
            double z = 400000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }

        private void Label18_Click(object sender, EventArgs e)
        {
            double z = 450000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }

        private void Label17_Click(object sender, EventArgs e)
        {
            double z = 500000;
            string bb = txttongtien.Text;
            double cc = Convert.ToDouble(bb);
            double aa = z - cc;
            textBox1.Text = Convert.ToString(aa);
        }
        #endregion

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}  
