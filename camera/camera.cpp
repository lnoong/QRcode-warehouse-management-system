#include"camera.h"  

Frame Tframe;
int nRet = MV_OK;
void* handle = NULL;
Mat NullPicture = imread(".\\resources\\picture\\null.png");
Mat image = NullPicture;
string result = "000";


bool PrintDeviceInfo(MV_CC_DEVICE_INFO* pstMVDevInfo)
{
	if (NULL == pstMVDevInfo)
	{
		printf("The Pointer of pstMVDevInfo is NULL!\n");
		return false;
	}
	if (pstMVDevInfo->nTLayerType == MV_GIGE_DEVICE)//判断相机的接入是通过ID还是通过USB
	{
		int nIp1 = ((pstMVDevInfo->SpecialInfo.stGigEInfo.nCurrentIp & 0xff000000) >> 24);
		int nIp2 = ((pstMVDevInfo->SpecialInfo.stGigEInfo.nCurrentIp & 0x00ff0000) >> 16);
		int nIp3 = ((pstMVDevInfo->SpecialInfo.stGigEInfo.nCurrentIp & 0x0000ff00) >> 8);
		int nIp4 = (pstMVDevInfo->SpecialInfo.stGigEInfo.nCurrentIp & 0x000000ff);

		// ch:打印当前相机ip和用户自定义名字 | en:print current ip and user defined name
		printf("Device Model Name: %s\n", pstMVDevInfo->SpecialInfo.stGigEInfo.chModelName);
		printf("CurrentIp: %d.%d.%d.%d\n", nIp1, nIp2, nIp3, nIp4);
		printf("UserDefinedName: %s\n\n", pstMVDevInfo->SpecialInfo.stGigEInfo.chUserDefinedName);
	}
	else if (pstMVDevInfo->nTLayerType == MV_USB_DEVICE)
	{
		printf("Device Model Name: %s\n", pstMVDevInfo->SpecialInfo.stUsb3VInfo.chModelName);
	}
	else
	{
		printf("Not support.\n");
	}

	return true;
}
int CameraOpen(int nIndex)
{

	MV_CC_DEVICE_INFO_LIST stDeviceList;
	memset(&stDeviceList, 0, sizeof(MV_CC_DEVICE_INFO_LIST));
	// 枚举设备
	// enum device
	nRet = MV_CC_EnumDevices(MV_GIGE_DEVICE | MV_USB_DEVICE, &stDeviceList);
	if (MV_OK != nRet)
	{
		return -1;//设备枚举失败
	}
	if (stDeviceList.nDeviceNum > 0)
	{
		for (int i = 0; i < stDeviceList.nDeviceNum; i++)
		{
			MV_CC_DEVICE_INFO* pDeviceInfo = stDeviceList.pDeviceInfo[i];
			if (NULL == pDeviceInfo)
			{
				break;
			}
			PrintDeviceInfo(pDeviceInfo);
		}
	}
	else
	{
		return -2;//找不到设备
	}

	if (nIndex >= stDeviceList.nDeviceNum)
	{
		return -3;//无该设备
	}

	// 选择设备并创建句柄
	// select device and create handle
	nRet = MV_CC_CreateHandle(&handle, stDeviceList.pDeviceInfo[nIndex]);
	if (MV_OK != nRet)
	{
		return -4;//创建设备句柄失败
	}

	// 打开设备
	// open device
	nRet = MV_CC_OpenDevice(handle);
	if (MV_OK != nRet)
	{
		return -5;//设备打开失败

	}

	nRet = MV_CC_SetEnumValue(handle, "TriggerMode", MV_TRIGGER_MODE_OFF);
	if (MV_OK != nRet)
	{
		return -6;//内触发模式设置失败
	}

	nRet = MV_CC_SetEnumValue(handle, "AcquisitionMode", 2);//Continuous:2 视频流模式连续采集,实时显示  SingleFrame:0 单帧采集模式,只输出一张图片
	if (MV_OK != nRet)
	{

		return -7;//连续设置采集失败
	}

	nRet = MV_CC_StartGrabbing(handle);
	if (nRet != 0)
	{
		return -8;//抓取失败
	}
	return 0;//设备打开成功
}
int CameraClose()
{
	nRet = MV_CC_StopGrabbing(handle);
	if (MV_OK != nRet)
	{
		return -1;//关闭取流失败
	}
	nRet = MV_CC_CloseDevice(handle);
	if (MV_OK != nRet)
	{
		return -2;//设备关闭失败
	}
	else
		nRet = MV_CC_DestroyHandle(handle);
	if (MV_OK != nRet)
	{
		return -3;//设备句柄销毁失败
	}
	return 0;//设备关闭成功
}
int CameraSet(double exposure = 15000, double gain = 17, double gamma = 0.7)
{
	MV_CC_StopGrabbing(handle);
	nRet = MV_CC_SetFloatValue(handle, "ExposureTime", exposure);
	if (nRet != MV_OK)
	{
		return -3;//曝光设置失败
	}

	nRet = MV_CC_SetFloatValue(handle, "Gain", gain);
	if (MV_OK != nRet)
	{
		return -4;//增益设置失败
	}

	nRet = MV_CC_SetBoolValue(handle, "GammaEnable", true);
	if (MV_OK != nRet)
	{
		return -5;//伽马校正未打开
	}

	nRet = MV_CC_SetFloatValue(handle, "Gamma", gamma);
	if (MV_OK != nRet)
	{
		return -6;//伽马校正设置失败
	}

	nRet = MV_CC_SetEnumValue(handle, "UserSetSelector", 1);//支持1、2、3组，3组不同参数保存
	if (MV_OK != nRet)
	{
		return -7;//保存失败
	}
	nRet = MV_CC_SetEnumValue(handle, "UserSetDefault", 1);//选择上电后，默认加载那一组参数
	if (MV_OK != nRet)
	{
		return -8;//加载失败
	}
	MV_CC_StartGrabbing(handle);
	CameraClose();
	return 0;
}
int CreateQR(char* objectId)
{
	string path = ".\\QRcode\\";
	string command = "mkdir " + path;
	if (_access(path.c_str(), 0) == -1)	//如果文件夹不存在
		system(command.c_str());

	string src = objectId;
	string QRName;
	QRName = path + src + ".jpg";
	QRcode* code = QRcode_encodeString(src.c_str(), 0, QR_ECLEVEL_H, QR_MODE_8, 1);
	Mat img = Mat(code->width, code->width, CV_8U);

	for (int i = 0; i < code->width; ++i) {
		for (int j = 0; j < code->width; ++j)
		{
			img.at<uchar>(i, j) = (code->data[i * code->width + j] & 0x01) == 0x01 ? 0 : 255;
		}
	}
	resize(img, img, Size(img.rows * 10, img.cols * 10), 0, 0, INTER_NEAREST);

	Mat result = Mat::zeros(img.rows + 30, img.cols + 30, CV_8U);
	//白底
	result = 255 - result;
	//转换成彩色
	cvtColor(result, result, COLOR_GRAY2BGR);
	cvtColor(img, img, COLOR_GRAY2BGR);
	//建立roi
	Rect roi_rect = Rect((result.rows - img.rows) / 2, (result.cols - img.rows) / 2, img.cols, img.rows);
	//roi关联到目标图像，并把源图像复制到指定roi
	img.copyTo(result(roi_rect));
	//imshow(src, result);
	imwrite(QRName, result);
	return 0;
}
static unsigned  int __stdcall PictureProcess(void* FrameClass)
{
	Frame Inside = *(Frame*)FrameClass;
	Mat src;
	Mat gray, binary, canny;
	int nRet = MV_OK;
	MV_FRAME_OUT stImageInfo = { 0 };
	//加载图片解码
	Ptr<wechat_qrcode::WeChatQRCode> detector;
	string detect_prototxt = ".\\resources\\model\\detect.prototxt";
	string detect_caffe_model = ".\\resources\\model\\detect.caffemodel";
	string sr_prototxt = ".\\resources\\model\\sr.prototxt";
	string sr_caffe_model = ".\\resources\\model\\sr.caffemodel";
	detector = makePtr<wechat_qrcode::WeChatQRCode>(detect_prototxt, detect_caffe_model, sr_prototxt, sr_caffe_model);
	waitKey(200);
	while (1)
	{
		nRet = MV_CC_GetImageBuffer(Inside.handle, &stImageInfo, INFINITE);//采集图片
		src = Mat(stImageInfo.stFrameInfo.nHeight, stImageInfo.stFrameInfo.nWidth, CV_8UC3, stImageInfo.pBufAddr);
		if (nRet == MV_OK)
		{
			cvtColor(src, gray, COLOR_BGR2GRAY);
			threshold(gray, binary, 0, 255, THRESH_BINARY | THRESH_OTSU);
			vector<Mat> vPoints;
			vector<String> strDecoded;
			strDecoded = detector->detectAndDecode(binary, vPoints);
			for (int i = 0; i < strDecoded.size(); i++)
			{
				*Inside.result = strDecoded[0];
				Point pt1 = Point((int)vPoints[i].at<float>(0, 0), (int)vPoints[i].at<float>(0, 1));
				Point pt2 = Point((int)vPoints[i].at<float>(1, 0), (int)vPoints[i].at<float>(1, 1));
				Point pt3 = Point((int)vPoints[i].at<float>(2, 0), (int)vPoints[i].at<float>(2, 1));
				Point pt4 = Point((int)vPoints[i].at<float>(3, 0), (int)vPoints[i].at<float>(3, 1));
				line(src, pt1, pt2, Scalar(0, 255, 0), 2);
				line(src, pt2, pt3, Scalar(0, 255, 0), 2);
				line(src, pt3, pt4, Scalar(0, 255, 0), 2);
				line(src, pt4, pt1, Scalar(0, 255, 0), 2);
				putText(src, strDecoded[i], pt1, 0, 0.5, Scalar(255, 0, 0), 2);
			}

			* Inside.outprt = src;
			nRet = MV_CC_FreeImageBuffer(Inside.handle, &stImageInfo);//释放缓存
		}
		else
		{
			*Inside.outprt = NullPicture;
			break;
		}
	}
	return 0;
}
void ImageToCs(unsigned char* data)
{
	Mat img = image;
	int channels = img.channels();
	int nRows = img.rows;
	int nCols = img.cols * channels;
	if (img.isContinuous())
	{
		nCols *= nRows;
		nRows = 1;
	}
	uchar* p;
	for (int i = 0; i < nRows; ++i)
	{
		p = img.ptr<uchar>(i);
		for (int j = 0; j < nCols; ++j)
		{
			data[i * nCols + j] = p[j];
		}
	}
}
const char* StrToCs()
{
	string str = result;
	const char* return_char;
	return_char = strdup(str.c_str());
	return return_char;
}
void DetectQR(int nIndex)
{
	CameraOpen(nIndex);
	Tframe.handle = handle;
	Tframe.outprt = &image;
	Tframe.result = &result;
	unsigned int nThreadID = 0;	
	void* hThreadHandle = (void*)_beginthreadex(NULL, 0, PictureProcess, &Tframe, 0, &nThreadID);

}

int SignIn(const char* userID,const char* password)
{
	const char* real_password = mysqluserselect(userID, "4");
	string real_password_s = real_password;
	string pass_s = password;
	string userID_s= userID;

	if (real_password_s == pass_s)
	{
		return 0;//登陆成功
	}
	else if (real_password_s == "NULL")
	{
		if (userID_s == "")
		{
			return -3;//请输入
		}
		return -1;//用户不存在
	}
	else
	{
		return -2;//密码错误
	}
}


int SignUp(const char* userName,const char* userID, const char* password, const char* password_retype,const char* email)
{
	const char* real_password = mysqluserselect(userID, "4");
	string real_password_s = real_password;
	string userName_s=userName;
	string userID_s=userID;
	string password_s = password;
	string password_retype_s= password_retype;
	string email_s = email;
	if (real_password_s != "NULL")
	{
		return -1;//用户已存在
	}
	if (real_password_s == "NULL")
	{
		if(!regex_match(userName_s, regex("^.{3,20}$")))
		{
			return -2;//用户名长度为3-20的所有字符
		}
		else if (!regex_match(userID_s, regex("1([0-9]{10})")))
		{
			return -3;//账号11位手机号
		}
		else if(!regex_match(password_s, regex("^(?=.*\\d)(?=.*[a-z]|[A-Z]).{8,18}$")))
		{
			return -4;//长度在8~18之间,大写或小写字母+数字+字符
		}
		else if (!regex_match(email_s, regex("([0-9A-Za-z\\-_\\.]+)@([0-9a-z]+\\.[a-z]{2,3}(\\.[a-z]{2})?)")))
		{
			return -5;//邮箱不合规
		}
		else
		{
			if (password_s != password_retype_s)
			{
				return -6;//密码不一致
			}
			else
			{
				mysqluseradd(userName, userID, email, "0", password);
				return 0;
			}

		}

	}

}

const char* ForgetPassword(const char* userID, const char* email)
{

	const char* real_email = mysqluserselect(userID, "2");
	string real_email_s = real_email;
	string email_s = email;
	const char* return_char = mysqluserselect(userID, "4");
	if (real_email_s == email_s)
	{
		string result = "您的密码是：" + (string)return_char;
		return result.c_str();//验证成功
	}
	else if (real_email_s == "NULL")
	{
		return "用户不存在";//用户不存在
	}
	else
	{
		return "验证失败";//验证失败
	}

	

}

void ReadPassword( char* remember,  char* userID,  char* password)
{
	CSimpleIniA ini;
	ini.LoadFile("config.ini");
	const char* remember_c = ini.GetValue("userinfo", "remember", "err");
	strcpy(remember, remember_c);
	const char* userID_c = ini.GetValue("userinfo", "userid", "err");
	strcpy(userID, userID_c);
	const char* password_c = ini.GetValue("userinfo", "pass", "err");
	strcpy(password, password_c);
}

void WritePassword(const char* remember, const char* userID, const char* password)
{
	CSimpleIniA ini;
	ini.LoadFile("config.ini");
	ini.SetValue("userinfo", "remember", remember);
	ini.SetValue("userinfo", "userid", userID);
	ini.SetValue("userinfo", "pass", password);
	ini.SaveFile("config.ini");
}

int  LinkMysql() 
{
	CSimpleIniA ini;
	ini.LoadFile("config.ini");
	const char* Server = ini.GetValue("database","Server","localhost");
	const char* User = ini.GetValue("database", "User", "root");
	const char* Password = ini.GetValue("database", "Password", "0819");
	const char* Database = ini.GetValue("database", "Database", "laopaoer");
	return mysqllink(Server, User, Password, Database);
	return 0;
}

int  ReLinkMysql(const char* Server, const char* User, const char* Password, const char* Database)
{
	CSimpleIniA ini;
	ini.LoadFile("config.ini");
	ini.SetValue("database", "Server", Server);
	ini.SetValue("database", "User", User);
	ini.SetValue("database", "Password", Password);
	ini.SetValue("database", "Database", Database);
	ini.SaveFile("config.ini");
	return mysqllink(Server, User, Password, Database);
}

int ObjectAdd(const char* objectName, const char* type, const char* place)
{
	CSimpleIniA ini;
	ini.LoadFile("config.ini");
	string lastid=ini.GetValue("objectinfo", "lastid", "000");
	int lastid_i = atoi(lastid.c_str());
	string objectname = objectName;
	char temp [4];
	sprintf(temp,"%03d", (lastid_i + 1));
	string objectid = temp;
	string type_s = type;
	string place_s = place;
	string state = "1";
	SYSTEMTIME sys;
	GetLocalTime(&sys);
	char tmp[30] = { NULL };
	sprintf(tmp, "%4d-%02d-%02d %02d:%02d:%02d", sys.wYear, sys.wMonth, sys.wDay, sys.wHour, sys.wMinute, sys.wSecond);
	string time = tmp;
	ini.SetValue("objectinfo", "lastid", objectid.c_str());
	ini.SaveFile("config.ini");
	mysqlobjectadd(objectname.c_str(), objectid.c_str(), type_s.c_str(), place_s.c_str(), state.c_str(), time.c_str());
	return 0;
}
const char* MysqlLinkInfo()
{

	CSimpleIniA ini;
	ini.LoadFile("config.ini");
	string Server = ini.GetValue("database", "Server", "localhost");
	string Port= ini.GetValue("database", "Port", "3306");
	string User = ini.GetValue("database", "User", "root");
	string Password = ini.GetValue("database", "Password", "0819");
	string Database = ini.GetValue("database", "Database", "laopaoer");
	string sql_s = "server=" + Server + "; port =" + Port + "; user =" + User + "; password =" + Password + "; database ="+ Database+";";
	return sql_s.c_str();
}
const char* UserInfo(const char* userid,const char* c)
{
	const char* info=mysqluserselect(userid, c);
	return info;
}

const char* ObjectInfo(const char* objectid, const char* c)
{
	const char* info = mysqlobjectselect(objectid, c);
	return info;
}