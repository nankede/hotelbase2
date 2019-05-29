using Component.Access;
using HotelBase.Entity.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBase.DataAccess.Distributor
{
    public class H_RelationAccess : BaseAccess<H_RelationModel>
    {
        public H_RelationAccess() : base(MysqlHelper.Db_HotelBase)
        {

        }
    }
}
