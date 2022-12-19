#pragma once
#include<iostream>
#include <stdio.h>
#include <string>
using namespace std;

extern "C" __declspec(dllimport) const char* ReceiveData(unsigned int m_DataSocket/*, char* DataBuf*/, int nlength);
extern "C" __declspec(dllimport) int Close(unsigned int m_DataSocket);
extern "C" __declspec(dllimport) unsigned int ConnectTest(const char* ulAddress, int iPort);
extern "C" __declspec(dllimport) int SendData(unsigned int m_DataSocket,const char* pcData, int iLength);
extern "C" __declspec(dllimport) unsigned int Connect();
extern "C" __declspec(dllimport) void ConnectSave(const char* ip, int port);
