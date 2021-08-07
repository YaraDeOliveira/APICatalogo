﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Pagination
{
    public class ProdutosParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                //_pageSize = (value > maxPageSize) ? maxPageSize : value;
                if (value > maxPageSize)
                    _pageSize = maxPageSize;
                else
                    _pageSize = value;
            }
            
        }

    }
}
