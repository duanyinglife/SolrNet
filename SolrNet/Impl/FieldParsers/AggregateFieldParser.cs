﻿#region license
// Copyright (c) 2007-2009 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Xml;
using SolrNet.Utils;

namespace SolrNet.Impl.FieldParsers {
    public class AggregateFieldParser : ISolrFieldParser {
        private readonly IEnumerable<ISolrFieldParser> parsers;

        public AggregateFieldParser(IEnumerable<ISolrFieldParser> parsers) {
            this.parsers = parsers;
        }

        public bool CanHandleSolrType(string solrType) {
            return Func.Any(parsers, p => p.CanHandleSolrType(solrType));
        }

        public bool CanHandleType(Type t) {
            return Func.Any(parsers, p => p.CanHandleType(t));
        }

        public object Parse(XmlNode field, Type t) {
            foreach (var p in parsers) {
                if (p.CanHandleType(t) && p.CanHandleSolrType(field.Name))
                    return p.Parse(field, t);
            }
            return null;
        }
    }
}