﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    [Serializable]
    public abstract class Base_DAO
    {
        #region Constants
        protected const string ID_PA = "@IDparam";

        protected const string CREATORID = "CreatedBy";
        protected const string CREATORID_PA = "@CreatorIDParam";

        protected const string CREATEDDATE = "CreatedDate";
        protected const string CREATEDDATE_PA = "@CreatedDateParam";

        protected const string MODIFIERID = "ModifiedBy";
        protected const string MODIFIERID_PA = "@ModifierIDParam";

        protected const string MOIDFIEDDATE = "ModifiedDate";
        protected const string MOIDFIEDDATE_PA = "@ModifiedDateParam";

        protected const string BASECOLUMNS = "[CreatedBy], "
                                           + "[CreatedDate], "
                                           + "[ModifiedBy], "
                                           + "[ModifiedDate] ";

        protected const string BASEMUSTCOLUMNS = "[CreatedBy], "
                                           + "[CreatedDate] ";
        #endregion
    }
}
