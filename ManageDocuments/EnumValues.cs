using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNCSCAPI.ManageDocuments
{
    public enum EnumQAReceiptParameter
    {
        PDS=1,
        PRIORITY = 2,
        TIDEOVER = 3,
        AAY = 4,
        SPLPDS = 5,
        CEMENT = 6,
        HOPURCHASE = 7,
        SEIZUR = 8,
        Total = 9,
        PTMGRNMP = 10,
        SGRY = 11,
        ANNAPURNA = 12,
        alFreeRice = 13,
        RECEIVEDFROM = 14,
        TRANSFERWITHINREGION = 15,
        TRANSFEROTHERREGION = 16,
        EXCESS = 17,
        CLEANINGANDPACKING = 18,
        VCFLOOD = 19,
        SALESRETURN = 20,
        TotalOtherReceipt = 21,
        TotalReceipt = 22,
        GrandTotalReceipt = 23,
        PURCHASE = 24,
        GUNNYRELEASE = 25,
        HULLING = 26
    }
    public enum EnumQAIssuesHeaderParameter
    {
        PDS =1,
        COOP = 2,
        POLICE = 3,
        NMP = 4,
        BULK = 5,
        CREDIT = 6,
        OAP = 7,
        SRILANKA = 8,
        AAY = 9,
        SPLPDS = 10,
        PDSCOOP = 11,
        CEMENTFLOOD = 12,
        TotalSales = 13,
        PTMGR = 14,
        SGRY = 15,
        ANNAPOORNA = 16,
        TotalFreeRiceIssues = 17,
        ISSUESTOPROCESSING = 18,
        TRANSFERWITHINREGION = 19,
        TRANSFEROTHERREGION = 20,
        WRITEOFF = 21,
        CLEANING = 22,
        VCBLG = 23,
        PURCHASERETURN = 24,
        TotalOtherIssues = 25,
        TotalIssues = 26,
        BalanceQty = 27,
        MENDING = 28,
        SALES = 29,
        GU = 30
    }
}
