# surfer

surfer是使用java語言來開發的網路與封包的框架  
  
網路部分使用Netty  
封包部分使用Google Protobuf  
  
客戶端目前提供C#版本(但是也可以用在Unity上)  
  
# 目錄說明

example 範例  
generateProto 產生proto的工具, 要配合protoc-3.0.0-win32一起使用  
protoc-3.0.0-win32 由Google提供的產生proto的編譯器  
surfer.client surfer的客戶端函式庫(C#)  
surfer.net surfer的網路函式庫(Java)  
surfer.server surfer的伺服器函式庫(Jave)  