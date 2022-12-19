#pragma once
extern "C" __declspec(dllimport) int mysqllink(const char* Server, const char* User, const char* Password, const char* Database);
extern "C" __declspec(dllimport) const char*  mysqluserselect(const char* userID, const char* column);
extern "C" __declspec(dllimport) int mysqluserchange(char* userID, char* input, char* column);
extern "C" __declspec(dllimport) int mysqluseradd(const char* username, const char* userID, const  char* email, const char* power, const char* password);
extern "C" __declspec(dllimport) int mysqluserdel(char* userID);

extern "C" __declspec(dllimport) const char* mysqlobjectselect(const char* objectID, const char* column);
extern "C" __declspec(dllimport) int mysqlobjectchange(char* objectID, char* input, char* column);
extern "C" __declspec(dllimport) int mysqlobjectadd(const char* objectname, const char* objectID, const char* type, const char* place, const char* state, const char* lasttime);
extern "C" __declspec(dllimport) int mysqlobjectdel(char* objectID);
