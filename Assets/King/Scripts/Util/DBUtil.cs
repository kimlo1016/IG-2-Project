using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEditor;
using System.IO;
using MySql.Data.MySqlClient;
using LitJson;


namespace Asset.MySql
{
    public enum ETableType
    {
        AccountDB,
        AccountInfoDB,
        Max
    };

    public enum EAccountColumns
    {
        Email,
        Password,
        Nickname,
        Max
    }

    public enum EAccountInfoColumns
    {
        Nickname,
        AccountData,
        Max
    }

    public class MySqlSetting
    {
        private static bool hasInit = false;

        private static string _connectionString;
        private static string[] _insertStrings = new string[(int)ETableType.Max];
        private static string _insertInitDataString;
        private static string _selectAccountString;
        private static string _selectJsonString;

        /// <summary>
        ///  MySql ���� �ʱ�ȭ
        /// </summary>
        public static void Init()
        {
            if (hasInit)
            {
                return;
            }

            Init(true);
        }

        /// <summary>
        /// MySql ������ �ʱ�ȭ
        /// </summary>
        /// <param name="isNeedReset"> �ʱ�ȭ�� �ʿ��ϸ� true, �ƴϸ� false</param>
        public static void Init(bool isNeedReset)
        {
            if (!isNeedReset)
            {
                return;
            }

            _connectionString = Resources.Load<TextAsset>("Connection").text;
            _insertStrings = Resources.Load<TextAsset>("Insert").text.Split('\n');
            _insertInitDataString = Resources.Load<TextAsset>("InitInsertAccountData").text;
            _selectAccountString = Resources.Load<TextAsset>("Select").text;

            SetEnum();
            Debug.Log("Enum Setting ��");
        }

        [MenuItem("Tools/GenerateEnum")]
        private static void SetEnum()
        {
            string settingString = Resources.Load<TextAsset>("SetEnum").text;
            string tableTypeString = settingString.Split('\n')[0];
            string columnTypeString = settingString.Split('\n')[1];

            List<string> tableNames = new List<string>();
            Dictionary<string, List<string>> columNames = new Dictionary<string, List<string>>();

            try
            {
                // DB���� ���̺�� �÷��� ��������
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    _sqlConnection.Open();

                    // ���̺� �� ��������
                    MySqlCommand tableTypeCommand = new MySqlCommand(tableTypeString, _sqlConnection);
                    MySqlDataReader tableTypeReader = tableTypeCommand.ExecuteReader();

                    while (true)
                    {
                        if (tableTypeReader.Read() == false)
                        {
                            break;
                        }

                        string tableName = tableTypeReader[0].ToString();
                        tableNames.Add(tableName);
                        columNames.Add(tableName, new List<string>());
                    }

                    tableTypeReader.Close();

                    // ���̺� �� ���� Column �� ��������
                    foreach (string table in tableNames)
                    {
                        string columnSelectString = columnTypeString + table + ";";

                        MySqlCommand columnCommand = new MySqlCommand(columnSelectString, _sqlConnection);
                        MySqlDataReader columnTypeReader = columnCommand.ExecuteReader();

                        while (true)
                        {
                            if (columnTypeReader.Read() == false)
                            {
                                break;
                            }

                            string columnName = columnTypeReader["Field"].ToString();
                            columNames[table].Add(columnName);
                        }
                        columnTypeReader.Close();
                    }

                    _sqlConnection.Close();
                }

                // �ش� ���뿡 �´� ���� �����ϱ�
                using (StreamWriter streamWriter = new StreamWriter("./Assets/Scripts/Util/MySqlEnum.cs"))
                {
                    // ��ó��
                    streamWriter.WriteLine("namespace Asset {");

                    // enum �����ϱ�
                    //  1. ���̺� Ÿ�� 
                    streamWriter.WriteLine("\tpublic enum ETableType {");
                    foreach (string table in tableNames)
                    {
                        streamWriter.WriteLine($"\t\t{table},");
                    }
                    streamWriter.WriteLine("\t}");

                    //  2. ���̺��� �÷� Ÿ��
                    foreach (string table in tableNames)
                    {
                        streamWriter.WriteLine($"\tpublic enum E{table}Columns {{");

                        foreach (string column in columNames[table])
                        {
                            streamWriter.WriteLine($"\t\t{column},");
                        }

                        streamWriter.WriteLine("\t}");
                    }

                    // ��ó��
                    streamWriter.WriteLine("}");
                }
                AssetDatabase.Refresh();
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return;
            }
        }

        /// <summary>
        /// ���� �߰��ϱ�
        /// </summary>
        /// <param name="Email">���� Email</param>
        /// <param name="Password">���� PW</param>
        /// <param name="Nickname">���� Nickname</param>
        /// <returns>���������� �Է��� �Ǿ��� ��� true, �ƴϸ� false
        /// (��ǥ������ email Nickname�� ��ĥ ��� false ��ȯ)</returns>
        public static bool AddNewAccount(string Email, string Password, string Nickname)
        {
            try
            {
                if (HasValue(EAccountColumns.Email, Email))
                {
                    throw new System.Exception("Email �ߺ���");
                }
                if (HasValue(EAccountColumns.Nickname, Nickname))
                {
                    throw new System.Exception("Nickname �ߺ���");
                }


                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertAccountString = GetInsertString(ETableType.AccountDB, Nickname, Password, Email);
                    MySqlCommand _insertAccountCommand = new MySqlCommand(_insertAccountString, _mysqlConnection);

                    string _insertAccountInfoString = GetInsertString(ETableType.AccountInfoDB, Nickname);
                    MySqlCommand _insertAccountInfoCommand = new MySqlCommand(_insertAccountInfoString, _mysqlConnection);

                    MySqlCommand _insertInitDataCommand = new MySqlCommand(_insertInitDataString + $"where {Nickname};", _mysqlConnection);

                    _mysqlConnection.Open();
                    _insertAccountCommand.ExecuteNonQuery();
                    _insertAccountInfoCommand.ExecuteNonQuery();
                    _insertInitDataCommand.ExecuteNonQuery();
                    _mysqlConnection.Close();
                }

                return true;
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }
        }
        private static string GetInsertString(ETableType tableType, params string[] values)
        {
            string insertString = _insertStrings[(int)tableType] + '(';

            foreach (string value in values)
            {
                insertString += $"'{value}',";
            }

            insertString = insertString.TrimEnd(',') + ");";

            return insertString;
        }

       


        /// <summary>
        /// �ش� ���� DB�� �ִ��� Ȯ���Ѵ�.
        /// </summary>
        /// <param name="columnType">Account ���̺��� ���ϱ� ���� colum ��</param>
        /// <param name="value">���� ��</param>
        /// <returns>���� �ִٸ� true, �ƴϸ� false�� ��ȯ�Ѵ�.</returns>
        public static bool HasValue(EAccountColumns columnType, string value)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    bool result = false;

                    string selectString = _selectAccountString + $" WHERE {columnType} = '{value}';";

                    _sqlConnection.Open();

                    MySqlCommand _selectCommand = new MySqlCommand(selectString, _sqlConnection);
                    MySqlDataReader _selectData = _selectCommand.ExecuteReader();

                    result = _selectData.Read();

                    _sqlConnection.Close();

                    return result;
                }
            }
            catch
            {
                Debug.LogError("������: Doublecheck");
                return false;
            }

        }

        /// <summary>
        /// AccountInfo Table���� baseType�� baseValue�� �������� checkType�� checkValue�� ��ġ�ϴ��� Ȯ����
        /// </summary>
        /// <param name="baseType">���� ������ Column Ÿ��</param>
        /// <param name="baseValue">���� ������ ��</param>
        /// <param name="checkType">Ȯ���� ������ Column Ÿ��</param>
        /// <param name="checkValue">Ȯ���� ��</param>
        /// <returns>��ġ�ϸ� true�� ��ȯ, �ƴϰų� ������ ���� ��� false ��ȯ</returns>
        public static bool CheckValueByBase(EAccountInfoColumns baseType, string baseValue,
            EAccountInfoColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.AccountInfoDB, baseType, baseValue, checkType, checkValue);
        }
        /// <summary>
        /// Account Table���� baseType�� baseValue�� �������� checkType�� checkValue�� ��ġ�ϴ��� Ȯ����
        /// ex. ID(baseType)�� aaa(baseValue)�� �������� Password(checkType)�� 123(checkValue)���� Ȯ����
        /// </summary>
        /// <param name="baseType">���� ������ Column Ÿ��</param>
        /// <param name="baseValue">���� ������ ��</param>
        /// <param name="checkType">Ȯ���� ������ Column Ÿ��</param>
        /// <param name="checkValue">Ȯ���� ��</param>
        /// <returns>��ġ�ϸ� true�� ��ȯ, �ƴϰų� ������ ���� ��� false ��ȯ</returns>
        public static bool CheckValueByBase(EAccountColumns baseType, string baseValue,
            EAccountColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.AccountDB, baseType, baseValue, checkType, checkValue);
        }
        private static bool CheckValueByBase<T>(ETableType targetTable, T baseType, string baseValue,
            T checkType, string checkValue) where T : System.Enum
        {
            string checkTargetValue = GetValueByBase(targetTable, baseType, baseValue, checkType);
            if (checkTargetValue != null)
            {
                return checkTargetValue == checkValue;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Account ���̺��� baseType�� baseValue�� �������� targetType�� �����͸� ������
        /// </summary>
        /// <param name="baseType">������ �Ǵ� ���� Column��</param>
        /// <param name="baseValue">������ �Ǵ� ������</param>
        /// <param name="targetType">�������� ���� ������ Column��</param>
        /// <returns>�ش� �����͸� ��ȯ. ���� �� null ��ȯ</returns>
        public static string GetValueByBase(EAccountColumns baseType, string baseValue, EAccountColumns targetType)
        {
            return GetValueByBase(ETableType.AccountDB, baseType, baseValue, targetType);
        }
       
        private static string GetValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string selectString = $"Select {targetType} from {targetTable} where {baseType} = '{baseValue}';";

                    _sqlConnection.Open();

                    MySqlCommand command = new MySqlCommand(selectString, _sqlConnection);
                    MySqlDataReader resultReader = command.ExecuteReader();

                    if (!resultReader.Read())
                    {
                        throw new System.Exception("base ���� ����");
                    }

                    string result = resultReader[targetType.ToString()].ToString();

                    _sqlConnection.Close();

                    return result;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return null;
            }

        }

       
        /// <summary>
        /// Account Table���� baseType�� baseValue�� �������� TargetType�� TargetValue�� ������
        /// </summary>
        /// <param name="baseType">���� ���� Column��</param>
        /// <param name="baseValue">���� ���� ������</param>
        /// <param name="targetType">������ ���� Column��</param>
        /// <param name="targetValue">������ ��</param>
        /// <returns>���������� ����Ǿ��ٸ� true, �ƴϸ� false�� ��ȯ</returns>
        public static bool UpdateValueByBase(EAccountColumns baseType, string baseValue,
            EAccountColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.AccountDB, baseType, baseValue, targetType, targetValue);
        }
        
        public static bool UpdateValueByBase(EAccountInfoColumns baseType, string baseValue,
            EAccountInfoColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.AccountInfoDB, baseType, baseValue, targetType, targetValue);
        }
        private static bool UpdateValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType, int targetValue) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateString = $"Update {targetTable} set {targetType} = {targetValue} where {baseType} = '{baseValue}';";
                    MySqlCommand command = new MySqlCommand(updateString, _sqlConnection);

                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
                    _sqlConnection.Close();

                    return true;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }


        }

        /// <summary>
        /// ������ ĳ���� �����͸� ���� Json�� �ҷ���.
        /// </summary>
        /// <param name="nickname"> ������ �г���</param>
        /// <returns>JsonData�� ��ȯ.</returns>
        public static JsonData SelectCharacterData(string nickname)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string selectString = $"Select * from AccountInfoDB where {nickname};";
                    MySqlCommand selectCommand = new MySqlCommand(selectString, _sqlConnection);
                    _sqlConnection.Open();
                    MySqlDataReader dataReader = selectCommand.ExecuteReader();
                    if(dataReader.Read())
                    {
                        _selectJsonString = dataReader["AccountData"].ToString();
                    }
                    JsonData resultData = JsonMapper.ToObject(_selectJsonString);
                    _sqlConnection.Close();

                    return resultData;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }
        }

        /// <summary>
        /// ������ ĳ���� ������ Json���� Ư�� ���� �ٲ���
        /// </summary>
        /// <param name="nickname">������ �г���</param>
        /// <param name="targetColumn">���� �ٲ��� Column��</param>
        /// <param name="value">�ٲ��� ��</param>
        /// <returns>�����ϸ� true, �����ϸ� false</returns>
        public static bool ReplaceCharacterData(string nickname, string targetColumn, string value)
        {
            try
            {
                using(MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string replaceString = $"Update AccountInfoDB set AccountData = json_replace(AccountData,'$.{targetColumn}', {value}) where {nickname};";

                    MySqlCommand replaceCommand = new MySqlCommand(replaceString, _sqlConnection);
                    _sqlConnection.Open();
                    replaceCommand.ExecuteNonQuery();
                    _sqlConnection.Close();

                    return true;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }
        }
        
    }

}