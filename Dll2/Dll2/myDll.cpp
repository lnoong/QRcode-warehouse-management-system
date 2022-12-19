#include "pch.h"
#include "myDll.h"
#include "framework.h"
#include <mysql.h>
#include <iostream>
#include <string>

using namespace std;

MYSQL mysql; //数据库句柄

/*连接数据库*/
int mysqllink(const char* Server = "localhost", const char* User = "root", const char* Password = "0819", const char* Database = "laopaoer") {
	
	mysql_init(&mysql); //句柄初始化
	if (!mysql_real_connect(&mysql, Server, User,Password, Database, 3306, NULL, 0)) //判断数据库是否连接成功
	{
		mysql_query(&mysql, "SET NAMES GB2312");
		mysql_library_init(0, NULL, NULL);//初始化MySQL库  
		//printf("Error connecting to database:%s\n", mysql_error(conn));
		return -1;//没连接成

	}
	else {
		mysql_query(&mysql, "SET NAMES GB2312");
		mysql_library_init(0, NULL, NULL);//初始化MySQL库  
		//printf("Connected...\n");
		return 0;//连接成
	}
	
}

/*用户查询*/
const char* mysqluserselect(const char* userID, const char*  column) {

	MYSQL_RES *result ;//查询结果集，存放查询结果  
	MYSQL_ROW sql_row;//存放一行查询结果的字符串数组  
	string temp;
	string id = userID;
	string scolumn = column;
	string select = "";
	const char* out;

	if (scolumn == "1") {//用户名
		select = "select username from user where userid = '" + id + "';";
	}
	if (scolumn == "2") {//邮箱
		select = "select email  from user where userid = '" + id + "';";
	}
	if (scolumn == "3") {//权限
		select = "select  power  from user where userid = '" + id + "';";
	
	}
	if (scolumn == "4") {//密码
		select = "select  password  from user where userid = '" + id + "';";
	}

	mysql_query(&mysql, select.c_str());//执行查询语句，mysql_query如果查询成功，零；如果出现一个错误，非零。  
	result = mysql_store_result(&mysql);//保存查询到的数据到result  
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
		if (&result != NULL) mysql_free_result(result);//释放结果资源  
	}
	return out;
}

/*用户修改*/
int mysqluserchange(char* userID, char* input, char* column) {
	string id = userID;
	string instr = input;
	string scolumn = column;
	string change = "";
	int res;

	if (scolumn == "1") {//改用户名
		change = "update user set username = '" + instr + "' where userid='" + id + "' ;";
	}
	if (scolumn == "2") {//改邮箱
		change = "update user set email = '" + instr + "' where userid='" + id + "' ;";
	}
	if (scolumn == "3") {//改权限
		change = "update user set power = '" + instr + "' where userid='" + id + "' ;";
	}
	if (scolumn == "4") {//改密码
		change = "update user set password = '" + instr + "' where userid='" + id + "' ;";
	}

	res = mysql_query(&mysql, change.c_str());
	return res;
}

/*用户增加*/
int mysqluseradd(const char* username, const char* userID, const  char* email, const char* power, const char* password) {
	string name = username;
	string id = userID;
	string pas = password;
	string em = email;
	string pow = power;
	string add = "";
	int res;
	//e控制权限
	add = "insert into user values('" + name + "','" + id + "',  '" + em + "', '" + pow + "','" + pas + "'); ";
	res = mysql_query(&mysql, add.c_str());
	return res;
}

/*用户删除*/
int mysqluserdel(char *userID)
{
	string id = userID;
	string change = "";
	int res;
	change = "delete from user where  userid = '" + id + "';";
	res = mysql_query(&mysql, change.c_str());
	return res;
}

/*物品查询*/
const char* mysqlobjectselect(const char* objectID, const char* column) {

	MYSQL_RES* result;//查询结果集，存放查询结果  
	MYSQL_ROW sql_row;//存放一行查询结果的字符串数组  
	string temp;
	string id = objectID;
	string scolumn = column;
	string select = "";
	const char* out;

	if (scolumn == "1") {//物品名
		select = "select objectname from object where objectid = '" + id + "';";
	}
	if (scolumn == "2") {//种类
		select = "select type from object where objectid = '" + id + "';";
	}
	if (scolumn == "3") {//位置
		select = "select place from object where objectid = '" + id + "';";

	}
	if (scolumn == "4") {//状态
		select = "select state from object where objectid = '" + id + "';";
	}
	if (scolumn == "4") {//时间
		select = "select lasttime from object where objectid = '" + id + "';";
	}

	mysql_query(&mysql, select.c_str());//执行查询语句，mysql_query如果查询成功，零；如果出现一个错误，非零。  
	result = mysql_store_result(&mysql);//保存查询到的数据到result  
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
		if (&result != NULL) mysql_free_result(result);//释放结果资源  
	}
	return out;
}

/*物品修改*/
int mysqlobjectchange(char* objectID, char* input, char* column) {
	string id = objectID;
	string instr = input;
	string scolumn = column;
	string change = "";
	int res;

	if (scolumn == "1") {//物品名
		change = "update object set objectname = '" + instr + "' where objectid='" + id + "' ;";
	}
	if (scolumn == "2") {//改种类
		change = "update object set type = '" + instr + "' where objectid='" + id + "' ;";
	}
	if (scolumn == "3") {//改位置
		change = "update object set place = '" + instr + "' where objectid='" + id + "' ;";
	}
	if (scolumn == "4") {//改状态
		change = "update object set satae = '" + instr + "' where objectid='" + id + "' ;";
	}
	if (scolumn == "4") {//改时间
		change = "update object set lasttime = '" + instr + "' where objectid='" + id + "' ;";
	}
	res = mysql_query(&mysql, change.c_str());
	return res;
}

/*物品增加*/
int mysqlobjectadd(const char* objectname, const char* objectID, const char* type, const char* place, const char* state, const char* lasttime) {
	string name = objectname;
	string id = objectID;
	string pas = type;
	string em = place;
	string pow = state;
	string time = lasttime;
	string add = "";
	int res;
	//e控制权限
	add = "insert into object values('" + name + "','" + id + "',  '" + pas + "', '" + em + "','" + pow + "','" + time + "'); ";
	res = mysql_query(&mysql, add.c_str());
	return res;
}

/*物品删除*/
int mysqlobjectdel(char* objectID)
{
	string id = objectID;
	string change = "";
	int res;
	change = "delete from object where  objectid = '" + id + "';";
	res = mysql_query(&mysql, change.c_str());
	return res;
}