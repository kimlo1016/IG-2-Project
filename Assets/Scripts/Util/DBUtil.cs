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
        CharacterDB,
        Max
    };

    public enum EAccountColumns
    {
        Email,
        Password,
        Nickname,
        Qustion,
        Answer,
        Max
    }

    public enum ECharacterColumns
    {
        Nickname,
        Gender,
        Tutorial,
        OnOff,
        Max
    }

    public enum ESocialStatus
    {
        None,
        Request,
        Friend,
        Block,
        Denied,
        Max
    }

    public class MySqlSetting
    {
        private static bool hasInit = false;

        private static string _connectionString;
        private static string[] _insertStrings = new string[(int)ETableType.Max];
        private static string _insertSocialRelationString;
        private static string _selectAccountString;
        private static string _selectSocialStatusString;
        private static string _updateSocialStatusString;

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
            _insertSocialRelationString = Resources.Load<TextAsset>("InsertSocial").text;
            _selectAccountString = Resources.Load<TextAsset>("Select").text;
            _selectSocialStatusString = Resources.Load<TextAsset>("SelectSocialStatus").text;
            _updateSocialStatusString = Resources.Load<TextAsset>("UpdateSocialStatus").text;

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
    

                    _mysqlConnection.Open();
                    _insertAccountCommand.ExecuteNonQuery();
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

        public static bool AddNewCharacter(string nickname, string gender)
        {
            try
            {
                
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertCharacterString = GetInsertString(ETableType.CharacterDB, nickname, gender);

                    MySqlCommand _insertCharacterCommand = new MySqlCommand(_insertCharacterString, _mysqlConnection);

                    _mysqlConnection.Open();
                    _insertCharacterCommand.ExecuteNonQuery();
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
        /// �Ҽ� ���� ��û�� DB�� �����
        /// </summary>
        /// <param name="requestNickname">��û�� ������ �г���</param>
        /// <param name="responseNickname">����� �Ǵ� ������ �г���</param>
        /// <param name="status">����� �Ҽ� ��� / ����</param>
        /// <returns>��Ͽ� �����ϸ� true, �ƴϸ� false </returns>
        public static bool RequestSocialInteraction(string requestNickname, string responseNickname, ESocialStatus status)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string insertSocialRequestString = _insertSocialRelationString + $"('{requestNickname}','{responseNickname}','{status}');";

                    MySqlCommand insertSocialRequestCommand = new MySqlCommand(insertSocialRequestString, _mysqlConnection);

                    _mysqlConnection.Open();
                    insertSocialRequestCommand.ExecuteNonQuery();
                    _mysqlConnection.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ư�� ��û�� ���� Status�� Ȯ����.
        /// </summary>
        /// <param name="requestNickname"> ��û�� Ȯ���� ������ �г��� </param>
        /// <param name="responseNickname">Ȯ���� ��û�� ����� �Ǵ� ������ �г���</param>
        /// <returns> �о���� Status�� Enum�� ��ȯ�ϰ�, ���� ã�� �� ���ٸ� ESocialStatus.None�� ��ȯ��. </returns>
        public static ESocialStatus CheckSocialStatus(string requestNickname, string responseNickname)
        {
           
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {

                    string selcetSocialRequestString = _selectSocialStatusString + $"where Requester = '{requestNickname}' and Repondent = '{responseNickname}';";

                    MySqlCommand selectSocialRequestCommand = new MySqlCommand(selcetSocialRequestString, _mysqlConnection);

                    _mysqlConnection.Open();

                    MySqlDataReader selectSocialStatusData = selectSocialRequestCommand.ExecuteReader();

                    if(!selectSocialStatusData.Read())
                    {
                        return ESocialStatus.None;
                    }

                    ESocialStatus resultStatus = (ESocialStatus)selectSocialStatusData.GetInt32("Status");

                    _mysqlConnection.Close();

                    return resultStatus;
                }
           
        }

        /// <summary>
        /// Ư�� ��û�� Status�� �ٲ���.
        /// </summary>
        /// <param name="requestNickname"> Status�� �ٲ��� ��û�� �� ������ �г���</param>
        /// <param name="responseNickname">��û�� ����� �Ǵ� ������ �г���</param>
        /// <param name="status"> �ٲ� ���� </param>
        /// <returns>�����ϸ� true, �����ϸ� false </returns>
        public static bool UpdateSocialStatus(string requestNickname, string responseNickname, ESocialStatus status)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateSocialStatusString = _updateSocialStatusString + $"'{status}' where Requester = '{requestNickname}' and Respondent = '{responseNickname}';";

                    MySqlCommand updateSocialStatusCommand = new MySqlCommand(updateSocialStatusString, _mysqlConnection);

                    _mysqlConnection.Open();
                    updateSocialStatusCommand.ExecuteNonQuery();
                    _mysqlConnection.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
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
        /// CharacterDB Table���� baseType�� baseValue�� �������� checkType�� checkValue�� ��ġ�ϴ��� Ȯ����
        /// </summary>
        /// <param name="baseType">���� ������ Column Ÿ��</param>
        /// <param name="baseValue">���� ������ ��</param>
        /// <param name="checkType">Ȯ���� ������ Column Ÿ��</param>
        /// <param name="checkValue">Ȯ���� ��</param>
        /// <returns>��ġ�ϸ� true�� ��ȯ, �ƴϰų� ������ ���� ��� false ��ȯ</returns>
        public static bool CheckValueByBase(ECharacterColumns baseType, string baseValue,
            ECharacterColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.CharacterDB, baseType, baseValue, checkType, checkValue);
        }
        /// <summary>
        /// AccountDB Table���� baseType�� baseValue�� �������� checkType�� checkValue�� ��ġ�ϴ��� Ȯ����
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
        /// AccountDB ���̺��� baseType�� baseValue�� �������� targetType�� �����͸� ������
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
        /// AccountDB Table���� baseType�� baseValue�� �������� TargetType�� TargetValue�� ������
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

        public static bool UpdateValueByBase(ECharacterColumns baseType, string baseValue,
            ECharacterColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.CharacterDB, baseType, baseValue, targetType, targetValue);
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


        public static bool DeleteRowByBase(ECharacterColumns baseType, string baseValue)
        {
            return DeleteRowByBase(ETableType.CharacterDB, baseType, baseValue);
        }
        private static bool DeleteRowByBase<T>(ETableType targetTable, T baseType, string baseValue) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string deleteString = $"Delete From {targetTable} where {baseType} = '{baseValue}';";
                    MySqlCommand command = new MySqlCommand(deleteString, _sqlConnection);

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
    }

}