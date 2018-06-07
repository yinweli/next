封包檔案以utf-8格式儲存, 相同系統內的封包通通放在同一個封包檔案中

檔案名稱規則
    系統名稱.proto
    例如:
        搖錢樹系統封包檔案名稱 : GoldPool.proto
        戰鬥系統封包檔案名稱 : Fight.proto

封包編號規則
    封包編號0~1000為保留編號, 不可使用
    每個系統的封包編號相隔100號
    例如:
        搖錢樹系統編號從1001開始
        戰鬥系統編號從1101開始

C# namespace名稱規則
    packet.系統名稱
    例如:
        搖錢樹系統 : packet.goldpool
        戰鬥系統 : packet.fight

java package名稱規則
    packet.系統名稱
    例如:
        搖錢樹系統 : packet.goldpool
        戰鬥系統 : packet.fight

java包裝類別名稱規則
    系統名稱Wapper
    例如:
        搖錢樹系統封包 : GoldPoolWapper
        戰鬥系統封包 : FightWapper