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
