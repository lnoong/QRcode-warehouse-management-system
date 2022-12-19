#pragma once
#include <iostream>
#include <Windows.h>
#include <time.h>
#include <io.h>
#include <process.h>
#include <string.h>
#include<opencv2/opencv.hpp>
#include "MvCameraControl.h"
#include <opencv2/wechat_qrcode.hpp>
#include "myDll.h"
#include <qrencode.h>
#include <regex>
#include "SimpleIni.h"

using namespace cv;
using namespace std;

class  Frame
{
public:
	void* handle = NULL;
	Mat* outprt = NULL;
	string* result = NULL;
};


unsigned int __stdcall PictureProcess(void* FrameClass);

extern "C" __declspec(dllimport) int CreateQR(char* objectId);
extern "C" __declspec(dllimport) void DetectQR(int nIndex);
extern "C" __declspec(dllimport) void ImageToCs(unsigned char* data);
extern "C" __declspec(dllimport) const char* StrToCs();


extern "C" __declspec(dllimport) int CameraOpen(int nIndex);//打开相机
extern "C" __declspec(dllimport) int CameraClose();//关闭相机
extern "C" __declspec(dllimport) int CameraSet(double exposure , double gain , double gamma);//相机设置


extern "C" __declspec(dllimport) int SignIn(const char* userID, const char* password);
extern "C" __declspec(dllimport) int SignUp(const char* userName, const char* userID, const char* password, const char*	password_retype, const char* email);

extern "C" __declspec(dllimport) const char* ForgetPassword(const char* userID, const char* email);
extern "C" __declspec(dllimport) int  LinkMysql();
extern "C" __declspec(dllimport) int  ReLinkMysql(const char* Server, const char* User, const char* Password, const char* Database);

extern "C" __declspec(dllimport)void ReadPassword(char* remember, char* userID, char* password);
extern "C" __declspec(dllimport)void WritePassword(const char* remember,const char* userID, const char* password);
extern "C" __declspec(dllimport)int ObjectAdd(const char* objectName, const char* type, const char* place);
extern "C" __declspec(dllimport) const char* MysqlLinkInfo();
extern "C" __declspec(dllimport) const char* UserInfo(const char* userid, const char* c);
extern "C" __declspec(dllimport) const char* ObjectInfo(const char* objectid, const char* c);