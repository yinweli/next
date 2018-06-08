# surfer

## 說明

surfer是使用java與C#來開發的網路與封包的框架  
網路部分使用Netty  
封包部分使用Google Protobuf  
伺服器提供java版本  
客戶端提供java與C#版本, C#版本可以用在Unity上  

## 目錄說明

### example
範例程式, 裡面包含java伺服器與C#客戶端  

### performanceTest
效能測試程式, 裡面包含java伺服器與java客戶端  

### generateProto
產生封包程式碼的工具, 要配合protoc-3.0.0-win32一起使用  

### protoc-3.0.0-win32
由Google提供的產生封包程式碼的編譯器  

### surfer.client
surfer的客戶端函式庫(C#)  

### surfer.net
surfer的網路函式庫(Java)  
裡面含有java伺服器與java客戶端網路元件  

### surfer.server
surfer的伺服器函式庫(Jave)  

## 效能測試

### 測試方式
客戶端採取送出ping封包, 然後伺服器收到後會回傳pong封包, 藉此測量送出到收到的時間  
客戶端在測量到時間後, 會在ping封包內附上上次測量的時間給伺服器紀錄  
伺服器每10秒會統計以下數據  
* 客戶端數量
* 伺服器收到ping封包次數
* ping封包來回花費時間
* 平均每個客戶端傳送ping封包次數
* 平均每個ping封包來回花費時間

### 測試機器(伺服器)
機器類型 : AWS t2.micro (on us-west-2b)  
作業系統 : Ubuntu server 16.04 64bit  

### 測試機器(客戶端)
CPU     : Intel Core i5-3450 @ 3.10GHz 3.50GHz  
RAM     : 12GB  
作業系統 : Windows10 Pro 64bit  

### 測試結果
每10秒  
* 客戶端數量 : 700
* 伺服器收到ping封包次數 : 42300
* ping封包來回花費時間 : 7003秒
* 平均每個客戶端傳送ping封包次數 : 60
* 平均每個ping封包來回花費時間 : 0.165秒

### 上限
當伺服器每10秒收到的ping封包超過43000個, 效能就會開始下降(與客戶端數量無關)  
