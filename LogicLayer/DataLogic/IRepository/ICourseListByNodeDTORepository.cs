﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
   public interface ICourseListByNodeDTORepository
    {
       List<CourseListByNodeDTO> GetAllByRootNodeId(int rootNodeId);
    }
}
