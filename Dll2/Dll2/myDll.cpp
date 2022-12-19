#include "pch.h"
#include "myDll.h"
#include "framework.h"
#include <mysql.h>
#include <iostream>
#include <string>

using namespace std;

MYSQL mysql; //���ݿ���

/*�������ݿ�*/
int mysqllink(const char* Server = "localhost", const char* User = "root", const char* Password = "0819", const char* Database = "laopaoer") {
	
	mysql_init(&mysql); //�����ʼ��
	if (!mysql_real_connect(&mysql, Server, User,Password, Database, 3306, NULL, 0)) //�ж����ݿ��Ƿ����ӳɹ�
	{
		mysql_query(&mysql, "SET NAMES GB2312");
		mysql_library_init(0, NULL, NULL);//��ʼ��MySQL��  
		//printf("Error connecting to database:%s\n", mysql_error(conn));
		return -1;//û���ӳ�

	}
	else {
		mysql_query(&mysql, "SET NAMES GB2312");
		mysql_library_init(0, NULL, NULL);//��ʼ��MySQL��  
		//printf("Connected...\n");
		return 0;//���ӳ�
	}
	
}

/*�û���ѯ*/
const char* mysqluserselect(const char* userID, const char*  column) {

	MYSQL_RES *result ;//��ѯ���������Ų�ѯ���  
	MYSQL_ROW sql_row;//���һ�в�ѯ������ַ�������  
	string temp;
	string id = userID;
	string scolumn = column;
	string select = "";
	const char* out;

	if (scolumn == "1") {//�û���
		select = "select username from user where userid = '" + id + "';";
	}
	if (scolumn == "2") {//����
		select = "select email  from user where userid = '" + id + "';";
	}
	if (scolumn == "3") {//Ȩ��
		select = "select  power  from user where userid = '" + id + "';";
	
	}
	if (scolumn == "4") {//����
		select = "select  password  from user where userid = '" + id + "';";
	}

	mysql_query(&mysql, select.c_str());//ִ�в�ѯ��䣬mysql_query�����ѯ�ɹ����㣻�������һ�����󣬷��㡣  
	result = mysql_store_result(&mysql);//�����ѯ�������ݵ�result  
	sql_row = mysql_fetch_row(result);
	int sign = mysql_num_rows(result);
	if (sign == 0)
	{
		temp = "NULL";
		out = _strdup(temp.c_str());
	}
	else
	{
		temp = sql_row[0];
		out = _strdup(temp.c_str());
		if (&result != NULL) mysql_free_result(result);//�ͷŽ����Դ  
	}
	return out;
}

/*�û��޸�*/
int mysqluserchange(char* userID, char* input, char* column) {
	string id = userID;
	string instr = input;
	string scolumn = column;
	string change = "";
	int res;

	if (scolumn == "1") {//���û���
		change = "update user set username = '" + instr + "' where userid='" + id + "' ;";
	}
	if (scolumn == "2") {//������
		change = "update user set email = '" + instr + "' where userid='" + id + "' ;";
	}
	if (scolumn == "3") {//��Ȩ��
		change = "update user set power = '" + instr + "' where userid='" + id + "' ;";
	}
	if (scolumn == "4") {//������
		change = "update user set password = '" + instr + "' where userid='" + id + "' ;";
	}

	res = mysql_query(&mysql, change.c_str());
	return res;
}

/*�û�����*/
int mysqluseradd(const char* username, const char* userID, const  char* email, const char* power, const char* password) {
	string name = username;
	string id = userID;
	string pas = password;
	string em = email;
	string pow = power;
	string add = "";
	int res;
	//e����Ȩ��
	add = "insert into user values('" + name + "','" + id + "',  '" + em + "', '" + pow + "','" + pas + "'); ";
	res = mysql_query(&mysql, add.c_str());
	return res;
}

/*�û�ɾ��*/
int mysqluserdel(char *userID)
{
	string id = userID;
	string change = "";
	int res;
	change = "delete from user where  userid = '" + id + "';";
	res = mysql_query(&mysql, change.c_str());
	return res;
}

/*��Ʒ��ѯ*/
const char* mysqlobjectselect(const char* objectID, const char* column) {

	MYSQL_RES* result;//��ѯ���������Ų�ѯ���  
	MYSQL_ROW sql_row;//���һ�в�ѯ������ַ�������  
	string temp;
	string id = objectID;
	string scolumn = column;
	string select = "";
	const char* out;

	if (scolumn == "1") {//��Ʒ��
		select = "select objectname from object where objectid = '" + id + "';";
	}
	if (scolumn == "2") {//����
		select = "select type from object where objectid = '" + id + "';";
	}
	if (scolumn == "3") {//λ��
		select = "select place from object where objectid = '" + id + "';";

	}
	if (scolumn == "4") {//״̬
		select = "select state from object where objectid = '" + id + "';";
	}
	if (scolumn == "4") {//ʱ��
		select = "select lasttime from object where objectid = '" + id + "';";
	}

	mysql_query(&mysql, select.c_str());//ִ�в�ѯ��䣬mysql_query�����ѯ�ɹ����㣻�������һ�����󣬷��㡣  
	result = mysql_store_result(&mysql);//�����ѯ�������ݵ�result  
	sql_row = mysql_fetch_row(result);
	int sign = mysql_num_rows(result);
	if (sign == 0)
	{
		temp = "NULL";
		out = _strdup(temp.c_str());
	}
	else
	{
		temp = sql_row[0];
		out = _strdup(temp.c_str());
		if (&result != NULL) mysql_free_result(result);//�ͷŽ����Դ  
	}
	return out;
}

/*��Ʒ�޸�*/
int mysqlobjectchange(char* objectID, char* input, char* column) {
	string id = objectID;
	string instr = input;
	string scolumn = column;
	string change = "";
	int res;

	if (scolumn == "1") {//��Ʒ��
		change = "update object set objectname = '" + instr + "' where objectid='" + id + "' ;";
	}
	if (scolumn == "2") {//������
		change = "update object set type = '" + instr + "' where objectid='" + id + "' ;";
	}
	if (scolumn == "3") {//��λ��
		change = "update object set place = '" + instr + "' where objectid='" + id + "' ;";
	}
	if (scolumn == "4") {//��״̬
		change = "update object set satae = '" + instr + "' where objectid='" + id + "' ;";
	}
	if (scolumn == "4") {//��ʱ��
		change = "update object set lasttime = '" + instr + "' where objectid='" + id + "' ;";
	}
	res = mysql_query(&mysql, change.c_str());
	return res;
}

/*��Ʒ����*/
int mysqlobjectadd(const char* objectname, const char* objectID, const char* type, const char* place, const char* state, const char* lasttime) {
	string name = objectname;
	string id = objectID;
	string pas = type;
	string em = place;
	string pow = state;
	string time = lasttime;
	string add = "";
	int res;
	//e����Ȩ��
	add = "insert into object values('" + name + "','" + id + "',  '" + pas + "', '" + em + "','" + pow + "','" + time + "'); ";
	res = mysql_query(&mysql, add.c_str());
	return res;
}

/*��Ʒɾ��*/
int mysqlobjectdel(char* objectID)
{
	string id = objectID;
	string change = "";
	int res;
	change = "delete from object where  objectid = '" + id + "';";
	res = mysql_query(&mysql, change.c_str());
	return res;
}