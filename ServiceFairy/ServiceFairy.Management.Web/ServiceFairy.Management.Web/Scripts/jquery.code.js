
(function (jq, undefined) {

    // 转换为大写
    function _toUpper(s) {
        return s.toUpperCase().replace(/\./g, '_');
    }

    // 是否为空引用或空串
    function _nullOrEmpty(s) {
        return s == null || s == '';
    }

    // 在换行符之前添加注释的前缀：“//”
    function _toNotes(s) {
        if (_nullOrEmpty(s)) return '';
        return s.replace(/\n+/g, '// ');
    }

    // 转换为参数变量名，例如：把UserName转换为username
    function _toVar(s) {
        if (s == null || s == '') return '';
        switch (s.toUpperCase()) {
            case 'USERNAME': return 'username';
            case 'USERID': return 'userid';
            case 'USERNAMES': return 'usernames';
        }

        return s.charAt(0).toLowerCase() + s.substring(1);
    }

    // 判断是否为数组
    function _isArray(name) {
        return (name != null && name.endsWith('[]'));
    }

    // 判断是否为字典
    function _isDict(name) {
        return (name != null && name.startsWith("{") && name.endsWith('}') && name.indexOf(',') >= 0);
    }

    // 获取字典类型名称的键与值类型
    function _getDictKeyValueTypes(name) {
        if (!_isDict(name)) return null;
        var k = name.indexOf(',');
        return [name.substr(1, k - 1), name.substr(k + 1, name.length - k - 2)];
    };

    var _objectCKeyWords = {};
    jq(['int', 'bool', 'char', 'short', 'long', 'unsigned', 'Byte', 'double', 'float', 'void',
        'for', 'switch', 'while', 'do', 'case', 'in', 'if', 'else',
        'register', 'auto', 'static', 'extern', 'const',
        'public', 'private', 'protected', 'class', 'struct']).each(function () { _objectCKeyWords[this] = true; });

    function _isObjectCKeyWord(name) {
        return !!_objectCKeyWords[name];
    }

    jq.extend({

        // 生成代码
        generateCode: function (cd, codeName, options) {  // codeName = <object-c|java>
            types = cd.types;
            var sb = jq.createSBuilder();

            // 转换为Object-c的变量名
            function _toObjectCVar(name) {
                name = _toVar(name);
                if (_isObjectCKeyWord(name)) name = '$' + name;
                return name;
            }

            function _getSrvName() {
                var s = cd.serviceName, k = s.indexOf('.');
                return (k < 0 ? s : s.substr(k + 1)).replace('.', '_');
            }

            // 转换为状态码的名称
            function _toStatusCodeName(name) {
                return 'SC_' + _toUpper(_getSrvName() + '_' + name);
            }

            // 转换为变量类型
            function _getVarType(shortName, name) {
                if (shortName == null || shortName == '') return 'id';
                if (_isArray(shortName)) return 'NSArray*';
                if (_isDict(shortName)) return "NSDictionary*";

                switch (shortName) {
                    case 'string': return 'NSString*';
                    case 'enum': return 'NSString*';
                    case 'boolean': return 'bool';
                    case 'char': return 'char';
                    case 'sbyte': return 'unsigned char';
                    case 'byte': return 'Byte';
                    case 'int16': return 'short';
                    case 'uint16': return 'unsigned short';
                    case 'int32': return 'int';
                    case 'uint32': return 'unsigned int';
                    case 'int64': return 'long long';
                    case 'uint64': return 'unsigned long long';
                    case 'single': return 'float';
                    case 'double': return 'double';
                    case 'decimal': return 'double';
                    case 'datetime': return 'NSDate*';
                    case 'guid': return 'NSString*';
                    case 'timespan': return 'NSDate*';
                }

                return 'Sf' + name + '*';
            }

            // 获取数组元素的类型
            function _getArrType(typeName) {
                typeName = _getElementType(typeName);
                switch (typeName.toLowerCase()) {
                    case 'string': case 'datetime': case 'timespan':
                    case 'guid': return 'NSString';

                    case 'boolean': case 'char': case 'sbyte': case 'byte': case 'int16': case 'uint16':
                    case 'int32': case 'uint32': case 'int64': case 'uint64': case 'single': case 'double':
                    case 'decimal': return 'NSNumber';
                }

                return typeName;
            }

            // 获取数组元素的类型
            function _getElementType(typeName) {
                if (typeName.endsWith('[]')) return typeName.substr(0, typeName.length - 2);
                return typeName;
            }

            // 获取属性修饰词
            function _getPropVerb(shortName) {
                if (_isArray(shortName)) return 'nonatomic, retain';

                switch (shortName) {
                    case 'guid': case 'datetime': case 'timespan':
                    case 'string': case 'enum': return 'nonatomic, copy';

                    case 'boolean': case 'char': case 'sbyte': case 'byte': case 'int16': case 'uint16': case 'int32':
                    case 'uint32': case 'int64': case 'uint64': case 'single': case 'double':
                    case 'decimal': return 'nonatomic, assign';
                }

                return 'nonatomic, retain';
            }

            function _isObj(shortName) {
                if (_isArray(shortName)) return true;
                switch (shortName) {
                    case 'guid': case 'datetime': case 'timespan':
                    case 'string': return true;
                    default: return false;
                }
            }

            function _getPropName(shortName) {
                if (_isArray(shortName)) return 'ARRAY';
                if (_isDict(shortName)) return 'DICT';

                switch (shortName) {
                    case 'string': return 'STRING';
                    case 'enum': return 'STRING';
                    case 'boolean': return 'BOOL';
                    case 'char': return 'CHAR';
                    case 'sbyte': return 'UCHAR';
                    case 'byte': return 'BYTE';
                    case 'int16': return 'SHORT';
                    case 'uint16': return 'USHORT';
                    case 'int32': return 'INT';
                    case 'uint32': return 'UINT';
                    case 'int64': return 'LONG';
                    case 'uint64': return 'ULONG';
                    case 'single': return 'FLOAT';
                    case 'double': return 'DOUBLE';
                    case 'decimal': return 'DOUBLE';
                    case 'datetime': return 'DATE';
                    case 'guid': return 'STRING';
                    case 'timespan': return 'DATE';
                }

                return 'OBJECT';
            }

            function _findCustomFlag(customFlags, name) {
                if (customFlags == null) return null;
                var r = jq(customFlags).first(function () { return this.Key == name; });
                return r == null ? null : r.Value;
            }

            function _findNewFields(f) {
                if (f.FieldDocs != null) {
                    var r = [];
                    jq(f.FieldDocs).each(function () { var v = _findNewFields(this); if (v != null) r.push({ type: v, field: this }); });
                    return r;
                } else {
                    return _findCustomFlag(f.CustomFlags, 'field');
                }
            }

            function _hasNewField(f) {
                return _findNewFields(f).length > 0;
            }

            function _toSplit(txt, summary) {
                if (!_nullOrEmpty(summary)) txt += ' (' + summary + ')';
                var count = (90 - txt.length) / 2;
                return ' '.paddingLeft(count, '/') + txt + ' '.paddingRight(count, '/');
            }

            options = options || { genIntf: true, genImpl: true, genSrvIntf: true, genSrvImpl: true, genDefination: true, genFactory: true };


            // 常量定义
            if (options.genDefination) {
                sb.appendLine(_toSplit('Defination'));

                // 接口名称
                sb.appendLine('\r\n// Command Names:');
                jq(cd.commands).where(function () { return !this.IsSys; }, true).each(function () {
                    var name = this.Name, srvName = _getSrvName();
                    sb.append('#define SM_').append(_toUpper(srvName)).append('_').append(_toUpper(name));
                    sb.append('    @"').append(cd.serviceName).append('/').append(name).appendLine('"');
                });

                // 消息名称
                sb.appendLine('\r\n// Message Names:');
                jq(cd.messages).each(function () {
                    var name = this.Name, srvName = _getSrvName();
                    sb.append('#define SM_').append(_toUpper(srvName)).append('_').append(_toUpper(name));
                    sb.append('    @"').append(cd.serviceName).append('/').append(name).appendLine('"');
                });

                sb.appendLine('\r\n// StatusCodes:');

                // 状态码
                jq(cd.statusCodes).each(function (k) {
                    sb.append('#define ').append(this.DefName).append('    (').append(this.Code).append(')');
                    sb.append('  // ').append(this.Title).appendLine();
                });
            }

            // 实体类工厂
            if (options.genFactory) {
                sb.appendLine(_toSplit('Entity Factory'));
                sb.append('SF_ENTITY_FACTORY_BEGIN(Sf').append(_getSrvName()).appendLine(')');

                // 实体类工厂描述
                sb.append('    SF_ENTITY_CREATOR_BEGIN');
                jq(cd.commands).where(function () { return !this.IsSys; }, true).each(function () {
                    var name = this.Name, requestType = _findEntityType(this.RequestTypeName), replyType = _findEntityType(this.ReplyTypeName);
                    var hasReq = !_isEmpty(requestType), hasRpy = !_isEmpty(replyType);
                    if (!hasReq && !hasRpy) return;
                    sb.append('\r\n        SF_ENTITY_CREATOR_').append(hasReq && hasRpy ? 'ALL' : hasReq ? 'REQUEST' : 'REPLY');
                    sb.append('(SM_').append(_toUpper(_getSrvName())).append('_').append(_toUpper(name)).append(', ');
                    sb.append(_getSrvName()).append('_').append(this.Name).append(')  // ').append(this.Title);
                });
                jq(cd.messages).each(function () {
                    var name = this.Name, requestType = _findEntityType(this.TypeName);
                    sb.append('\r\n        SF_ENTITY_CREATOR_MESSAGE');
                    sb.append('(SM_').append(_toUpper(_getSrvName())).append('_').append(_toUpper(name)).append(', ');
                    sb.append(_getSrvName()).append('_').append(this.Name).append(') // ').append(this.Title);
                });
                sb.appendLine('\r\n    SF_ENTITY_CREATOR_END\r\n');

                // 状态码
                sb.append('    SF_ENTITY_STATUS_DESC_BEGIN');
                jq(cd.statusCodes).each(function (k) {
                    sb.append('\r\n        SF_DEF_SC_DESC(').append(this.DefName);
                    sb.append(', @"').append(this.Title).append('")');
                });
                sb.appendLine('\r\n    SF_ENTITY_STATUS_DESC_END');

                sb.appendLine('SF_ENTITY_FACTORY_END');
            }

            // 实体类
            if (options.genImpl || options.genIntf) {
                jq(types).each(function () {
                    var type = this, typeName = 'Sf' + type.Name;
                    if (type.TypeShortName != 'object') return;
                    sb.append('\r\n').append(_toSplit(typeName)).append('\r\n\r\n');

                    if ((codeName = codeName.toLowerCase()) == 'object-c') {

                        if (options.genIntf) {
                            sb.append('// ').appendLine(type.Summary);

                            // 参数宏定义
                            sb.append('#define SF_').append(_toUpper(type.Name)).append('_ARGLIST ');
                            sb.appendLine(jq(type.FieldDocs).select(function (k) {
                                var varName = _toObjectCVar(this.Name);
                                return (k == 0 ? '' : varName) + ':(' + _getVarType(this.TypeShortName, this.TypeName) + ') ' + varName;
                            }).join(' '));

                            // 参数调用列表宏定义
                            sb.append('#define SF_').append(_toUpper(type.Name)).append('_INVOKELIST ');
                            sb.appendLine(jq(type.FieldDocs).select(function (k) {
                                var varName = _toObjectCVar(this.Name);
                                return (k == 0 ? '' : varName) + ':' + varName;
                            }).join(' '));

                            // @interface部分
                            sb.append('@interface Sf').append(type.Name).append(' : SfEntity')
                            .append(_hasNewField(type) ? '<ISfUserModifier>' : '').appendLine('\r\n');
                            sb.append('SF_DECLARE_ENTITY_INIT(SF_').append(_toUpper(type.Name)).appendLine('_ARGLIST)\r\n');
                            jq(type.FieldDocs).each(function (k) {
                                var f = this;
                                sb.append('// ').append(f.Summary);
                                if (_isArray(f.TypeShortName))
                                    sb.append(' (Array of ').append(_getElementType(f.TypeName)).append(')');
                                else if (_isDict(f.TypeShortName)) {
                                    var typeArr = _getDictKeyValueTypes(f.TypeName), keyType = typeArr[0], valueType = typeArr[1];
                                    sb.append(' (Dict of ').append(keyType).append(', ').append(valueType).append(')');
                                }
                                sb.append('\r\n@property (').append(_getPropVerb(f.TypeShortName)).append(') ');
                                sb.append(_getVarType(f.TypeShortName, f.TypeName)).append(' ').append(f.Name).appendLine(';');
                                sb.appendLine();
                            });
                            sb.appendLine('@end\r\n');
                        }

                        // @implementation 部分
                        if (options.genImpl) {
                            sb.append('// ').appendLine(type.Summary);
                            sb.append('@implementation ').append(typeName).appendLine();
                            if (type.FieldDocs.length > 0) {
                                sb.append('@synthesize ').append(jq(type.FieldDocs).select(function () { return this.Name; }).join(', ')).appendLine(';\r\n');
                            }
                            sb.append('SF_BEGIN_IMPLEMENT_ENTITY_INIT(').append(typeName).append(', SF_').append(_toUpper(type.Name)).append('_ARGLIST, ');
                            sb.append('SF_').append(_toUpper(type.Name)).append('_INVOKELIST)\r\n');
                            sb.append(jq(type.FieldDocs).select(function () { return '    self.' + this.Name + ' = ' + _toObjectCVar(this.Name) + '; // ' + this.Summary; }).join('\r\n'));
                            sb.append('\r\nSF_END_IMPLEMENT_ENTITY_INIT\r\n\r\n');

                            sb.append('SF_BEGIN_TYPE(').append(typeName).append(', @"Sf').append(type.Name).appendLine('")');
                            sb.append(jq(type.FieldDocs).select(function () {
                                var fName = this.Name, fTypeName = type.TypeName, typeShortName = this.TypeShortName;
                                var postfix = _getPropName(typeShortName), s;
                                if (postfix == 'OBJECT') {
                                    s = 'SF_PROP_OBJECT(' + typeName + ', ' + fName + ', @"' + fName + '", Sf' + this.TypeName + ')';
                                } else if (postfix == 'ARRAY') {
                                    var fTypeName2 = _getElementType(this.TypeName), postfix2 = _getPropName(_getElementType(this.TypeShortName));
                                    if (postfix2 == 'OBJECT') {
                                        s = 'SF_PROP_ARRAY_OBJECT(' + typeName + ', ' + fName + ', @"' + fName + '", Sf' + fTypeName2 + ')';
                                    } else if (postfix2 == 'ARRAY') {
                                        s = 'ERROR: 不支持二维数组';
                                    } else {
                                        s = 'SF_PROP_ARRAY_' + postfix2 + '(' + typeName + ', ' + fName + ', @"' + fName + '")';
                                    }
                                } else if (postfix == 'DICT') {
                                    var typeArr = _getDictKeyValueTypes(this.TypeName), keyTypeName = typeArr[0], valueTypeName = typeArr[1];
                                    s = 'SF_PROP_DICT(' + typeName + ', ' + fName + ', @"' + fName + '")';
                                } else {
                                    s = 'SF_PROP_' + postfix + '(' + typeName + ', ' + fName + ', @"' + fName + '")';
                                }

                                return '    ' + s + ' // ' + this.Summary;
                            }).join('\r\n'));
                            sb.appendLine('\r\nSF_END_TYPE\r\n');

                            jq(_findNewFields(type)).each(function () {
                                var t = this.type, f = this.field;
                                if (t == 'new_username' || t == 'new_password') {
                                    sb.append('- (NSString*) ').append((t == 'new_username') ? 'getNewUserName' : 'getNewPassword').appendLine(' {');
                                    sb.append('    return self.').append(f.Name).appendLine(';');
                                    sb.appendLine('}\r\n');
                                }
                            });

                            sb.appendLine('- (void) dealloc {');
                            sb.append(jq(type.FieldDocs).where(function () { return _isObj(this.TypeShortName); }, true).select(function () {
                                return '    [' + this.Name + ' release];';
                            }).join('\r\n'));
                            sb.appendLine('\r\n    [super dealloc];');
                            sb.appendLine('}');

                            sb.appendLine('\r\n@end\r\n');
                        }
                    } else if (codeName == 'java') {

                    }
                });
            }

            // 服务接口与实现
            if (options.genSrvImpl || options.genSrvIntf) {

                if (sb.length > 0) sb.append('\r\n\r\n');

                function _findEntityType(typeName) {
                    return jq(types).first(function () { return this.TypeName == typeName; });
                }

                function _isEmpty(type) {
                    return type == null || type.FieldDocs == null || type.FieldDocs.length == 0;
                }

                function _appendParamDocs(type, prefix) {
                    prefix = prefix || '- ';
                    jq(type == null ? null : type.FieldDocs).each(function () {
                        var f = this, shortName = f.TypeShortName, isObj = shortName == 'object' || shortName == 'object[]';
                        sb.append('// ').append(prefix).append(f.Name).append(' <');
                        sb.append(isObj ? f.TypeName : shortName).append('>');
                        sb.append(': ').append(f.Summary).appendLine();
                        if (isObj) {
                            var fType = _findEntityType(_getElementType(f.TypeName));
                            _appendParamDocs(fType, prefix + '  ');
                        }
                    });
                }

                if (codeName == 'object-c') {

                    if (options.genSrvIntf) {

                        sb.append('SF_BEGIN_DECLARE_SERVICE(Sf').append(_getSrvName()).appendLine('Service)\r\n');

                        // 所有接口
                        jq(cd.commands).where(function () { return !this.IsSys; }, true).each(function () {
                            var name = this.Name, requestType = _findEntityType(this.RequestTypeName), replyType = _findEntityType(this.ReplyTypeName);
                            sb.appendLine(_toSplit('Command ' + name));

                            sb.append('// Summary: ').appendLine(_toNotes(this.Title));
                            sb.append('// Remarks: ').appendLine(_toNotes(this.Desc));
                            sb.append('// Security: ').appendLine((this.SecurityLevel == null) ? '(无)' : (this.SecurityLevel + '').joinDesc(this.SecurityDesc));
                            sb.append('// Input: ').appendLine(!_isEmpty(requestType) ? requestType.Name : '(无)');
                            _appendParamDocs(requestType);
                            sb.append('// Output: ').appendLine(!_isEmpty(replyType) ? (replyType.Name + ' (sfData.Entity)') : '(无)');
                            _appendParamDocs(replyType);
                            sb.append('SF_DECLARE_INVOKER_METHODS');
                            if (!_isEmpty(requestType)) { // 有参数
                                sb.append('(').append(name).append(', Sf').append(requestType.Name);
                                sb.append(', SF_').append(_toUpper(requestType.Name) + '_ARGLIST').append(');');
                            } else { // 无参数
                                sb.append('_NOARGS(').append(name).append(');');
                            }

                            sb.appendLine('\r\n\r\n');
                        });

                        // 所有消息
                        jq(cd.messages).each(function () {
                            var name = this.Name, requestType = _findEntityType(this.TypeName);
                            sb.appendLine(_toSplit('Message ' + name));
                            sb.append('// Summary: ').appendLine(_toNotes(this.Title));
                            sb.append('// Remarks: ').appendLine(_toNotes(this.Desc));
                            sb.append('// Data: ').append(requestType.Name).appendLine(' (sfData.Entity)');
                            _appendParamDocs(requestType);
                            sb.append('SF_DECLARE_RECEIVER(').append(name).appendLine(')');
                            sb.appendLine('\r\n\r\n');
                        });

                        sb.append('SF_END_DECLARE_SERVICE(Sf').append(_getSrvName()).appendLine('Service)');
                    }

                    if (options.genSrvImpl) {
                        sb.append('SF_BEGIN_IMPLEMENT_SERVICE(Sf').append(_getSrvName()).appendLine('Service)\r\n');

                        // 所有接口
                        jq(cd.commands).where(function () { return !this.IsSys; }, true).each(function () {
                            var name = this.Name, requestType = _findEntityType(this.RequestTypeName), replyType = _findEntityType(this.ReplyTypeName);
                            sb.appendLine(_toSplit('Command ' + name));
                            sb.append('// Summary: ').appendLine(this.Title);
                            sb.append('SF_IMPLEMENT_INVOKER_METHODS');
                            if (!_isEmpty(requestType)) { // 有参数
                                sb.append('(').append(name).append(', Sf').append(this.RequestTypeName);
                                sb.append(', SM_').append(_toUpper(_getSrvName())).append('_').append(_toUpper(name));
                                sb.append(', SF_').append(_toUpper(requestType.Name) + '_ARGLIST').append(', SF_');
                                sb.append(_toUpper(requestType.Name) + '_INVOKELIST)');
                            } else {  // 无参数
                                sb.append('_NOARGS(').append(name);
                                sb.append(', SM_').append(_toUpper(_getSrvName())).append('_').append(_toUpper(name)).append(')');
                            }

                            sb.appendLine('\r\n\r\n');
                        });

                        // 所有消息
                        jq(cd.messages).each(function () {
                            var name = this.Name, requestType = _findEntityType(this.TypeName);
                            sb.appendLine(_toSplit('Message ' + name));
                            sb.append('// Summary: ').appendLine(this.Title);
                            sb.append('SF_IMPLEMENT_RECEIVER(').append(name).append(', SM_');
                            sb.append(_toUpper(_getSrvName())).append('_').append(_toUpper(name)).append(')');
                            sb.appendLine('\r\n\r\n');
                        });

                        sb.append('SF_END_IMPLEMENT_SERVICE(Sf').append(_getSrvName()).appendLine('Service)\r\n');
                    }
                }
            }

            return sb.toString();
        }
    });

})(jQuery);