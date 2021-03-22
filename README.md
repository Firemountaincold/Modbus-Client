# Modbus-Client
支持TCP、RTU、UDP的Modbus客户端工具。
## 以ModbusTCP client为基础的改进。   
使用System.IO.Ports进行串口通信。   
重写了示例代码函数。 
实现了在串口通信自动生成CRC校验码的功能。   
注意：传递CRC时使用uint是因为使用int时最高位为1会变为负数。  
实现了串口接收服务器回复的数据。  
注意：read方法运行后bytestoread会归0，所以使用时定义要在read前面。  
使用Socket完成UDP的发送和接收。
## 窗口：
![image](https://github.com/Firemountaincold/Modbus-Client/blob/main/Image.png)

## 更新文档： 
### 2021.3.18： 
#### 1.0： 
增加了RTU串口通信功能。 
#### 1.1： 
新增了RTU模式下功能码信息的支持。
### 2021.3.19：
#### 1.2：
现在可以正确的添加CRC校验码。  
现在可以正确的收到RTU模式下服务器的回复。
#### 1.3：
加入了UDP收发的功能。
### 2021.3.20：
#### 1.4：
修复了有关UDP连接导致程序崩溃的两个bug。
### 2021.3.22：
#### 1.5：
完善了UDP功能，现在用501端口接受消息。
