#include "pch.h"
#include<winsock.h>
#include<iostream>
#include "txdydll.h"
#include"SimpleIni.h"
using namespace std;

//勿忘，链接dll的lib
#pragma comment(lib, "ws2_32.lib")

const char* ReceiveData(unsigned int m_DataSocket, int nlength)
{
    /*memset(DataBuf, 0x00, nlength);*/
    char buf[100]={0};
    string ss;
    const char* rec;
    recv(m_DataSocket, buf, nlength, 0);
    ss = buf;
    rec = _strdup(ss.c_str());
    return rec;
}

int Close(unsigned int m_DataSocket)
{
    closesocket(m_DataSocket);
    WSACleanup();
    return TRUE;
}
unsigned int Connect()
{
    CSimpleIniA ini;
    ini.LoadFile("config.ini");
    const char* ulAddress = ini.GetValue("PLC", "ip", "127.0.0.1");
    const char* Port = ini.GetValue("PLC", "port", "4000");
    int iPort = atoi(Port);
    struct sockaddr_in addr;
    //加载套接字
    WORD wVersionRequested;
    WSADATA wsaData;
    int err;
    SOCKET m_DataSocket;
    wVersionRequested = MAKEWORD(1, 1);
    err = WSAStartup(wVersionRequested, &wsaData);
    if (err != 0)
    {
        return FALSE;
    }
    if (LOBYTE(wsaData.wVersion) != 1 || HIBYTE(wsaData.wVersion) != 1)
    {
        WSACleanup();
        return FALSE;
    }
    if ((m_DataSocket = socket(AF_INET, SOCK_STREAM, 0)) == -1)
    {
        cout << "创建socket号失败!" << endl;
        return FALSE;
    }

    addr.sin_family = AF_INET;
    addr.sin_port = htons(iPort);
    addr.sin_addr.s_addr = inet_addr(ulAddress);

    if (connect(m_DataSocket, (struct sockaddr*)&addr, sizeof(struct sockaddr)) == -1)
    {
        cout << "连接服务器失败!" << endl;
        return FALSE;
    }
    return m_DataSocket;
}
unsigned int ConnectTest(const char* ulAddress, int iPort)
{
    struct sockaddr_in addr;
    //加载套接字
    WORD wVersionRequested;
    WSADATA wsaData;
    int err;
    SOCKET m_DataSocket;
    wVersionRequested = MAKEWORD(1, 1);
    err = WSAStartup(wVersionRequested, &wsaData);
    if (err != 0)
    {
        return FALSE;
    }
    if (LOBYTE(wsaData.wVersion) != 1 || HIBYTE(wsaData.wVersion) != 1)
    {
        WSACleanup();
        return FALSE;
    }
    if ((m_DataSocket = socket(AF_INET, SOCK_STREAM, 0)) == -1)
    {
        cout << "创建socket号失败!" << endl;
        return FALSE;
    }

    addr.sin_family = AF_INET;
    addr.sin_port = htons(iPort);
    addr.sin_addr.s_addr = inet_addr(ulAddress);

    if (connect(m_DataSocket, (struct sockaddr*)&addr, sizeof(struct sockaddr)) == -1)
    {
        cout << "连接服务器失败!" << endl;
        return FALSE;
    }
    return m_DataSocket;
}

void ConnectSave(const char* ip, int port)
{
    CSimpleIniA ini;
    string port_s = to_string(port);
    ini.LoadFile("config.ini");
    ini.SetValue("PLC", "ip", ip);
    ini.SetValue("PLC", "port", port_s.c_str());
    ini.SaveFile("config.ini");
}

int SendData(unsigned int m_DataSocket,const char* pcData, int iLength)
{
    int iData = send(m_DataSocket, pcData, iLength, 0);
    if (iData <= 0)
    {
        cout << "数据发送失败!请确认!" << endl;
    }
    return iData;
}