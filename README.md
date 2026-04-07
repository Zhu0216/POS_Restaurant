# POS_Restaurant 🍴

這是一個基於 **C# WinForms** 開發的餐飲點餐系統 (POS)。本專案模擬了餐飲業的日常營運流程，包含菜單選購、訂單計算、帳單生成及歷史紀錄查詢，展示了桌面應用程式開發與資料處理的完整邏輯。

## 🚀 專案亮點 (Key Features)

*   **直覺式點餐介面**：使用 C# WinForms 打造，支援多類別餐點選擇，並具備即時總金額計算功能。
*   **訂單管理系統**：
    *   **購物車邏輯**：實現餐點增加、刪除及數量調整之邏輯判定。
    *   **折扣與計算**：內建自動計算稅額或折扣邏輯。
*   **資料持久化 (Persistence)**：整合 **SQLite** 本地資料庫，儲存詳細的歷史訂單紀錄，確保營運數據不遺失。
*   **報表與文件輸出**：具備將訂單明細導出為技術文件或明細格式的功能，模擬真實收銀流程。

## 🛠️ 技術棧 (Tech Stack)

*   **Language:** C# (.NET Framework / .NET Core)
*   **UI Framework:** WinForms (Windows Forms)
*   **Database:** SQLite
*   **Version Control:** Git

## 📂 專案結構 (Project Structure)

```text
├── Form1.cs            # 主要點餐介面邏輯
├── Models/             # 資料模型 (如 FoodItem, Order)
├── Data/               # 資料庫存取層 (SQLite Helper)
├── Restaurant.db       # SQLite 資料庫檔案
└── README.md           # 專案技術說明
