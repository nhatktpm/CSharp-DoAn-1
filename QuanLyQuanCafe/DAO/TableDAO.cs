﻿using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 80;
        public static int TableHeight = 80;

        private TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTabel @idTable1 , @idTabel2", new object[] { id1, id2 });
        }
        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public DataTable taibanan()
        {
            return DataProvider.Instance.ExecuteQuery("select * from dbo.TableFood");
        }

        public bool InsertTable(string name)
        {
            string query = string.Format("INSERT dbo.TableFood ( name )VALUES  ( N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateTable(int idTableFood, string name, string status)
        {
            string query = string.Format("UPDATE dbo.TableFood SET name = N'{0}', status = {1} WHERE id = {2}", name, status, idTableFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteTable(int idTable)
        {
           

            string query = string.Format("Delete TableFood where id = {0}", idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
