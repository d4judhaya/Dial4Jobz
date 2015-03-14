using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Enums
{
    public enum PaymentMode
    {
        CreditDebitCard = 1,
        ElectronicTransfer = 2,
        Cheque = 3,
        DemandDraft = 4,
        PickupCash = 5,
        CashDeposit=6,
        InterBank=7,
        NEFT=8,
        IMPS=9,
        CollectAtOffice=10

    }
}