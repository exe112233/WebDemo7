using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebDemo7.publics.MyClass
{
    public static class TransformToList
    {
        /// <summary>         
        /// DataSet转换为泛型集合         
        /// </summary>         
        /// <typeparam name="T">泛型类型</typeparam>         
        /// <param name="ds">DataSet数据集</param>         
        /// <param name="tableIndex">待转换数据表索引,默认第0张表</param>         
        /// <returns>返回泛型集合</returns>         
        public static List<T> ToList<T>(this DataSet ds, int tableIndex = 0)
        {

            if (ds == null || ds.Tables.Count < 0) return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;

            if (tableIndex < 0)
                tableIndex = 0;

            DataTable dt = ds.Tables[tableIndex];

            // 返回值初始化             
            List<T> result = new List<T>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        // 属性与字段名称一致的进行赋值                         
                        if (pi.Name.Equals(dt.Columns[i].ColumnName))
                        {

                            // 数据库NULL值单独处理                             
                            if (dt.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, dt.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }


        /// <summary>
        /// DataRow 转成 模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ToModel<T>(this DataRow dr) where T : class, new()
        {
            T ob = new T();
            if (dr != null)
            {
                Type vType = typeof(T);
                //创建一个属性的列表
                PropertyInfo[] prlist = vType.GetProperties();


                DataColumnCollection vDataCoulumns = dr.Table.Columns;
                try
                {
                    foreach (PropertyInfo vProInfo in prlist)
                    {
                        if (vDataCoulumns.IndexOf(vProInfo.Name) >= 0 && dr[vProInfo.Name] != DBNull.Value)
                        {
                            vProInfo.SetValue(ob, dr[vProInfo.Name], null);
                        }
                    }
                }
                catch (Exception ex)
                {


                }
            }
            return ob;
        }

    }
}