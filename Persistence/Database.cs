using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using Leaves_FAT_Management.Common;

namespace Leaves_FAT_Management.Persistence
{
    class Database
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["leavesConnectionString"].ConnectionString;

        /// <summary>
        /// Insert into table Leave
        /// </summary>
        /// <param name="idRessource"></param>
        /// <param name="idLeaveType"></param>
        /// <param name="leaveDate"></param>
        /// <returns></returns>
        public static int InsertLeave(int idRessource, int idLeaveType, DateTime leaveDate, bool halfDay)
        {
            int result = -1;
            OleDbConnection conn = new OleDbConnection(connectionString);
            
            try
            {
                conn.Open();
                string command1 = 
                    string.Format("INSERT INTO Leave(IDRessource, IDLeaveType, LeaveDate, Amount) VALUES('{0}', '{1}', '{2}', '{3}');",
                    idRessource, idLeaveType, leaveDate, halfDay ? 0.5 : 1.0); // data
                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;
                result = command.ExecuteNonQuery();   
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Return all ressources
        /// </summary>
        /// <returns></returns>
        public static DataSet SelectAllRessources()
        {
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                
                conn.Open();
                string command1 =
                    string.Format("SELECT CStr(Ressource.ID) + '|' +  Project.Projectname + '|' + Project.CPEmail + '|' +  IIf(Project.CPEmailCC Is Null, '', Project.CPEmailCC) + '|' + IIf(Project.CPForename Is Null, '', Project.CPForename) + '|' +  IIf(Project.CPSurname Is Null, '', Project.CPSurname) AS ID, Ressource.Forename + ' ' + Ressource.Surname AS Name " +
                                    "FROM Project INNER JOIN Ressource ON Project.ID = Ressource.Project " +
                                    "WHERE Ressource.IsActive=True " +
                                    "ORDER BY Ressource.Forename, Ressource.Surname;"); // query
                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;

                //set any parameters here
                OleDbDataAdapter da = new OleDbDataAdapter();

                da.SelectCommand = command;

                da.Fill(ds, "Ressource");
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        /// <summary>
        /// Select all leave types
        /// </summary>
        /// <returns></returns>
        public static DataSet SelectAllLeaveType()
        {
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {

                conn.Open();
                string command1 =
                    string.Format("SELECT ID, LeaveName FROM LeaveType ORDER BY ID;"); // query
                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;

                //set any parameters here
                OleDbDataAdapter da = new OleDbDataAdapter();

                da.SelectCommand = command;

                da.Fill(ds, "LeaveType");
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }
        
        /// <summary>
        /// select all leaves within the month / period
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static DataSet SelectAllLeaveByPeriod(DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string command1 =
                    string.Format("SELECT Ressource.Forename + ' ' + Ressource.Surname AS Name, Leave.LeaveDate AS `Date`, LeaveType.LeaveName AS `Leave type`, Project.Projectname AS `Project name`, Leave.ID AS `ID Leave`, Leave.Amount " +
                                    "FROM Project INNER JOIN (LeaveType INNER JOIN (Ressource INNER JOIN Leave ON Ressource.ID = Leave.IDRessource) ON LeaveType.ID = Leave.IDLeaveType) ON Project.ID = Ressource.Project " +
                                    "WHERE Leave.LeaveDate BETWEEN #{0}# AND #{1}#" +
                                    "AND Ressource.IsActive=True " +
                                    "ORDER BY Ressource.Forename, Leave.LeaveDate DESC;", from.ToString("MM/dd/yyyy"), to.ToString("MM/dd/yyyy")); // query                

                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;

                //set any parameters here
                OleDbDataAdapter da = new OleDbDataAdapter();

                da.SelectCommand = command;

                da.Fill(ds, "Leave");

                // loop through ds/dt to change `Leave type`
                if (ds != new DataSet() && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["Amount"].ToString()) && Convert.ToDouble(dr["Amount"]) == 0.5)
                        {
                            dr["Leave type"] = string.Format("{0} (½ day)", dr["Leave type"]);
                        }
                    }

                    // remove column Amount by reference
                    dt.Columns.Remove("Amount");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        /// <summary>
        /// count all the leaves taken by all ressources
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static DataSet CountLeaveRessource(DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string command1 =
                    string.Format("SELECT Ressource.Forename + ' ' + Ressource.Surname AS Name, " +
                                    "(SELECT IIF(ISNULL(SUM(Leave.Amount)), 0, SUM(Leave.Amount)) as `Local` from Leave " +
                                    "WHERE Leave.LeaveDate BETWEEN #{0}# AND #{1}# " +
                                    "AND Ressource.ID = Leave.IDRessource AND " +
                                    "(Leave.IDLeaveType = 1 OR Leave.IDLeaveType = 6 OR Leave.IDLeaveType = 7 OR Leave.IDLeaveType = 8 OR Leave.IDLeaveType = 9 OR Leave.IDLeaveType = 10)) as `Local`, " +
                                    "(SELECT IIF(ISNULL(SUM(Leave.Amount)), 0, SUM(Leave.Amount)) as `Sick` from Leave " +
                                    "WHERE Leave.LeaveDate BETWEEN #{0}# AND #{1}#" +
                                    "AND Ressource.ID = Leave.IDRessource AND Leave.IDLeaveType = 2) as `Sick` " +
                                    "FROM Ressource " +
                                    "WHERE Ressource.IsActive=True " +
                                    "ORDER BY Ressource.Forename;", from.ToString("MM/dd/yyyy"), to.ToString("MM/dd/yyyy")); // query                
                
                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;

                //set any parameters here
                OleDbDataAdapter da = new OleDbDataAdapter();

                da.SelectCommand = command;

                da.Fill(ds, "LeaveFAT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("error : {0} \n\n {1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        /// <summary>
        /// Delete a leave entry
        /// </summary>
        /// <param name="idRessource"></param>
        /// <param name="idLeaveType"></param>
        /// <param name="leaveDate"></param>
        /// <returns></returns>
        public static int DeletetLeave(object[] idLeave)
        {
            int result = -1;
            OleDbConnection conn = new OleDbConnection(connectionString);

            string idToDelete = string.Join(", ", idLeave);

            try
            {
                conn.Open();
                string command1 =
                    string.Format("DELETE FROM Leave WHERE ID IN ({0});", idToDelete); // data
                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Select all LDAP Username ( lowercased)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> SelectAllLDAPUsername()
        {
            DataSet ds = new DataSet();
            Dictionary<string, string> ldapUsername = new Dictionary<string, string>();
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string command1 =
                    string.Format("SELECT UCASE(Ressource.Surname) AS Surname, LCASE(Ressource.LDAPUsername) AS LDAPUsername FROM Ressource;"); // query
                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;

                //set any parameters here
                OleDbDataAdapter da = new OleDbDataAdapter();

                da.SelectCommand = command;

                da.Fill(ds, "RessourceLDAP");

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ldapUsername.Add(dr["Surname"].ToString(), 
                            dr["LDAPUsername"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return ldapUsername;
        }

        /// <summary>
        /// select the monthly leaves for all ressources
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static DataSet SelectMonthlyLeave(DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string command1 =
                    string.Format("SELECT Ressource.Forename + ' ' + Ressource.Surname AS Name, Leave.LeaveDate AS `Date`, Left(LeaveType.LeaveName, 1) AS `Leave type` " +
                                    "FROM LeaveType INNER JOIN (Ressource INNER JOIN Leave ON Ressource.ID = Leave.IDRessource) ON LeaveType.ID = Leave.IDLeaveType " +
                                    "WHERE Leave.LeaveDate BETWEEN #{0}# AND #{1}#" +
                                    "AND Ressource.IsActive=True " +
                                    "ORDER BY Ressource.Forename, Leave.LeaveDate;", from.ToString("MM/dd/yyyy"), to.ToString("MM/dd/yyyy")); // query                

                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;

                //set any parameters here
                OleDbDataAdapter da = new OleDbDataAdapter();

                da.SelectCommand = command;

                da.Fill(ds, "MonthlyLeave");
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        /// <summary>
        /// Insert into table holiday (batch insert for a given year)
        /// </summary>
        /// <param name="listHolidays"></param>
        /// <returns>Number of holidays inserted</returns>
        public static int InsertHolidays(List<Utility.Holiday> listHolidays)
        {
            int result = -1;
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string baseInsertCommand = "INSERT INTO Holiday(HolidayDate, HolidayYear, HolidayName) VALUES('{0}', '{1}', '{2}');";
                string command1 = String.Empty;

                foreach (Utility.Holiday holiday in listHolidays)
                {
                    command1 = String.Format(baseInsertCommand, holiday.HolidayDate, holiday.HolidayYear, holiday.HolidayName);

                    OleDbCommand command = new OleDbCommand(command1, conn);
                    command.Connection = conn;
                    command.CommandText = command1;
                    if (result == -1)
                    {
                        // first
                        result = command.ExecuteNonQuery();
                    }
                    else
                    {
                        // all others
                        result += command.ExecuteNonQuery();
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.Message + " >> " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Return all holidays by year 
        /// </summary>
        /// <param name="year">Year for which query is run</param>
        /// <returns></returns>
        public static DataSet SelectHolidayByYear(int year)
        {
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string command1 =
                    string.Format("SELECT Holiday.HolidayName AS `Name`, FORMAT(Holiday.HolidayDate, '(ddd) dd mmm yyyy') AS `Date`, Holiday.ID AS `HolidayID` " +
                                    "FROM Holiday WHERE Holiday.HolidayYear={0} " +
                                    "ORDER BY Holiday.HolidayDate;", year); // query

                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;

                //set any parameters here
                OleDbDataAdapter da = new OleDbDataAdapter();

                da.SelectCommand = command;

                da.Fill(ds, "Holiday");
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        /// <summary>
        /// Return all holidays' date (only) by year 
        /// </summary>
        /// <param name="year">Year for which query is run</param>
        /// <returns>List of all dates</returns>
        public static List<DateTime> SelectHolidayDateByYear(int year)
        {
            DataSet ds = new DataSet();
            List<DateTime> holidays = new List<DateTime>();
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string command1 =
                    string.Format("SELECT Holiday.HolidayDate " +
                                    "FROM Holiday WHERE Holiday.HolidayYear={0}", year); // query

                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;

                //set any parameters here
                OleDbDataAdapter da = new OleDbDataAdapter();

                da.SelectCommand = command;

                da.Fill(ds, "Holiday");

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        holidays.Add(Convert.ToDateTime(dr["HolidayDate"]));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return holidays;
        }

        /// <summary>
        /// Delete all holidays for a given year
        /// </summary>
        /// <param name="year">Year for which delete is run</param>
        /// <returns>Number of holidays deleted</returns>
        public static int DeletetHolidayByYear(int year)
        {
            int result = -1;
            OleDbConnection conn = new OleDbConnection(connectionString);
            
            try
            {
                conn.Open();
                string command1 =
                    string.Format("DELETE FROM Holiday WHERE Holiday.HolidayYear={0};", year); // data
                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Delete a holiday entry
        /// </summary>
        /// <param name="idHoliday">Holiday ID to be deleted</param>
        /// <returns></returns>
        public static int DeleteHoliday(int idHoliday)
        {
            int result = -1;
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string command1 = string.Format("DELETE FROM Holiday WHERE Holiday.ID = {0};", idHoliday); // data
                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Update a holiday entry
        /// </summary>
        /// <param name="holiday">Holiday object to be updated</param>
        /// <returns></returns>
        public static int UpdateHoliday(Utility.Holiday holiday)
        {
            int result = -1;
            OleDbConnection conn = new OleDbConnection(connectionString);

            try
            {
                conn.Open();
                string baseInsertCommand = "UPDATE Holiday SET Holiday.HolidayDate = '{0}', Holiday.HolidayYear = '{1}', Holiday.HolidayName = '{2}' WHERE Holiday.ID = {3};";
                string command1 = String.Empty;

                command1 = String.Format(baseInsertCommand, holiday.HolidayDate, holiday.HolidayYear, holiday.HolidayName, holiday.HolidayID);

                OleDbCommand command = new OleDbCommand(command1, conn);
                command.Connection = conn;
                command.CommandText = command1;
                result = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.Message + " >> " + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return result;

        }
    }
}
