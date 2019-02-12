
/*@requres jquery-1.4.1.js */

(function (jq) {
    jQuery.fn.extend({

        // 显示树
        showCommandTree: function (doc) {
            doc = jq(doc);
            return jq(this).each(function () {

                var e = jq(this);
                _post("/Command/GetAppServiceDocs", null, function (result) {

                    e.empty();
                    jq(result).each(function () {
                        var info = this, serviceName = info.Name, serviceVersion = info.Version;
                        var li = jq('<li><a class="service_title" href="javascript:void(0)"></a></li>').appendTo(e), div;
                        jq('.service_title', li).text(info.Name).toggle(function () {
                            if (!div) {
                                div = jq('<div><ul style="list-style-type:disk;"></ul></div>'), ul = jq('ul', div).css({ padding: 0, margin: 0, marginLeft: 10 });
                                var cmdDiv, msgDiv, statusCodeDiv;

                                // 接口
                                jq('<li><a class="item_title" href="javascript:void(0)">Command</a></li>').appendTo(ul).toggle(function () {
                                    var e = jq(this);
                                    if (!cmdDiv) {
                                        _post("/Command/GetAppCommands", { serviceName: info.Name, serviceVersion: info.Version }, function (r) {
                                            cmdDiv = jq('<div><ul style="list-style-type:circle"></ul></div>'), cmdUl = jq('ul', cmdDiv).css({ padding: 0, margin: 0, marginLeft: 10 });
                                            e.after(cmdDiv);
                                            jq(r).each(function () {
                                                var name = this.Name, ver = this.Version, title = this.Title, isSys = this.IsSys;
                                                if (isSys) return;
                                                jq('<li><a class="' + this.Usable.toLowerCase() + ' list_item_title" href="javascript:void(0)"></a></li>').appendTo(cmdUl).find('a').text(name).attr('title', title).click(function () {
                                                    // 显示接口文档
                                                    var cmdArg = { serviceName: serviceName, serviceVersion: serviceVersion, commandName: name, commandVersion: ver };
                                                    _post('/Command/GetAppCommandDocs', cmdArg, function (d) {
                                                        doc.empty();
                                                        var titleNode = jq('<div class="doc_panel_headline"></div>').text(serviceName + '/' + d.CommandDesc.Name).appendTo(doc);
                                                        if (d.UsableDoc.Usable != 'Normal') jq('<span class="' + d.UsableDoc.Usable.toLowerCase() + ' notice"></span>').innerText('  (' + d.UsableDoc.Desc + ' )').appendTo(titleNode);
                                                        _addPanel('摘要', _nil(d.Summary, '（无）'), doc, true);
                                                        _addPanel('详述', _nil(d.Remarks, '（无）'), doc, true);
                                                        _addPanel('安全', ((c = d.SecurityDoc) == null || c.Level == null) ? '（无）' : (c.Level + '').joinDesc(c.Desc), doc, true);
                                                        _showArg('输入参数', d.Input, doc, serviceName);
                                                        _showArg('输出参数', d.Output, doc, serviceName);
                                                    });
                                                });
                                            });
                                        });
                                    } else { cmdDiv.show(); }
                                }, function () { cmdDiv.hide(); });

                                // 消息
                                jq('<li><a class="item_title" href="javascript:void(0)">Message</a></li>').appendTo(ul).toggle(function () {
                                    var e = jq(this);
                                    if (!msgDiv) {
                                        _post("/Command/GetAppMessages", { serviceName: info.Name, serviceVersion: info.Version }, function (r) {
                                            msgDiv = jq('<div><ul style="list-style-type:circle"></ul></div>'), msgUl = jq('ul', msgDiv).css({ padding: 0, margin: 0, marginLeft: 10 });
                                            e.after(msgDiv);
                                            jq(r).each(function () {
                                                var name = this.Name, ver = this.Version, title = this.Title, desc = this.Desc;
                                                jq('<li><a class="list_item_title" href="javascript:void(0)"></a></li>').appendTo(msgUl).find('a').text(name).attr('title', title).click(function () {
                                                    // 显示消息文档
                                                    var msgArg = { serviceName: serviceName, serviceVersion: serviceVersion, messageName: name, messageVersion: ver };
                                                    _post('/Command/GetAppMessageDocs', msgArg, function (d) {
                                                        doc.empty();
                                                        jq('<div class="doc_panel_headline"></div>').text(serviceName + '/' + d.MessageDesc.Name).appendTo(doc);
                                                        _addPanel('摘要', _nil(d.Summary, '（空）'), doc, true);
                                                        _addPanel('详述', _nil(d.Remarks, '（空）'), doc, true);
                                                        _showArg('数据', d.Data, doc, serviceName);
                                                    });
                                                });
                                            });
                                        });
                                    } else { msgDiv.show(); }
                                }, function () { msgDiv.hide(); });

                                // 状态码
                                jq('<li><a class="item_title" href="javascript:void(0)">StatusCode</a></li>').appendTo(ul).click(function () {
                                    var e = jq(this);
                                    function _createRow(name, code, category, title, serviceCode, innerCode, desc) {
                                        var tr = jq('<tr><td class="status_name"></td><td class="status_code"></td><td class="status_category"></td>'
                                               + '<td class="status_title"></td><td class="status_serviceCode"></td>'
                                               + '<td class="status_innerCode"></td><td class="status_desc"></td></tr>');
                                        jq('.status_name', tr).text(name); jq('.status_code', tr).text(code); jq('.status_category', tr).text(category);
                                        jq('.status_title', tr).text(title); jq('.status_desc', tr).text(desc);
                                        jq('.status_serviceCode', tr).text(serviceCode); jq('.status_innerCode', tr).text(innerCode);
                                        return tr;
                                    }

                                    _post('/Command/GetAppStatusCodes', { serviceName: info.Name, serviceVersion: info.Version }, function (r) {
                                        doc.empty();

                                        jq('<div class="doc_panel_headline" style="margin-bottom:20px;"></div>').text(serviceName + ' 状态码').appendTo(doc);

                                        // 状态码定义
                                        var tbl = jq('<table class="doc_status_code_table"><thead></thead><tbody></tbody></table>').appendTo(jq('.doc_panel_content', _addPanel('定义', '', doc, true)));
                                        _createRow('名称', '状态码', '类型', '摘要', '服务编号', '内部编号', '详述').appendTo(jq('thead', tbl));

                                        var rIndex = 0;
                                        jq(r).each(function () {
                                            _createRow(this.Name, this.Code, this.Category, this.Title, this.ServiceCode, this.InnerCode, this.Desc)
                                                .appendTo(jq('tbody', tbl)).addClass('row_index_' + (rIndex++) % 2);
                                        });

                                        // 状态码代码生成
                                        var p = jq('.doc_panel_content', _addPanel('代码生成', '', doc, true));
                                    });
                                });

                                // 实体类代码
                                var codeDiv;
                                jq('<li><a class="item_entity_code item_title" href="javascript:void(0)">Code Generation</a></li>').appendTo(ul).toggle(function () {
                                    if (codeDiv == null) {
                                        var e = jq(this), cd;
                                        codeDiv = jq('<div><ul style="list-style-type:circle"></ul></div>'), codeUl = jq('ul', codeDiv).css({ padding: 0, margin: 0, marginLeft: 10 });
                                        e.after(codeDiv);

                                        function _getEntities(callback, cp) {
                                            var arg = { serviceName: serviceName, serviceVersion: serviceVersion };
                                            if (cd == null) _post('/Command/GetCodeGenerationData', arg, function (r) { cd = r; jq.extend(cd, arg); callback(r, cp); });
                                            else callback(cd, cp);
                                        }

                                        jq([{
                                            name: 'Obj-C(Entity-@interface)', title: 'Object-C(Entity - @interface)', func: function (cd, cp) {
                                                cp.innerText(jq.generateCode(cd, 'object-c', { genIntf: true }));
                                            }
                                        }, {
                                            name: 'Obj-C(Entity-@implementation)', title: 'Object-C(Entity - @implementation)', func: function (cd, cp) {
                                                cp.innerText(jq.generateCode(cd, 'object-c', { genImpl: true }));
                                            }
                                        }, {
                                            name: 'Obj-C(Srv-@interface)', title: 'Object-C(Service - @interface)', func: function (cd, cp) {
                                                cp.innerText(jq.generateCode(cd, 'object-c', { genSrvIntf: true }));
                                            }
                                        }, {
                                            name: 'Obj-C(Srv-@implementation)', title: 'Object-C(Service - @implementation)', func: function (cd, cp) {
                                                cp.innerText(jq.generateCode(cd, 'object-c', { genSrvImpl: true }));
                                            }
                                        }, {
                                            name: 'Obj-C(Defination)', title: 'Object-C(Defination)', func: function (cd, cp) {
                                                cp.innerText(jq.generateCode(cd, 'object-c', { genDefination: true }));
                                            }
                                        }, {
                                            name: 'Obj-C(Entity-Factory)', title: 'Object-C(Entity - Factory)', func: function (cd, cp) {
                                                cp.innerText(jq.generateCode(cd, 'object-c', { genFactory: true }));
                                            }
                                        }]).each(function () {
                                            var name = this.name, title = this.title, func = this.func;
                                            jq('<li><a class="list_item_title" href="javascript:void(0)"></a></li>').appendTo(codeUl).find('a').text(name).click(function () {
                                                doc.empty();
                                                jq('<div class="doc_panel_headline" style="margin-bottom:20px;"></div>').text(title).appendTo(doc);
                                                var codePanel = jq('<div class="doc_panel_content_code"></div>').appendTo(jq('.doc_panel_content', _addPanel('代码', '', doc, true)));
                                                _getEntities(func, codePanel);
                                            });
                                        });
                                    } else { codeDiv.show(); }
                                }, function () { codeDiv.hide(); });

                                li.after(div);
                            }
                            div.show();
                        }, function () {
                            div.hide();
                        });
                    });
                });
            });
        }
    });

    // 显示区域
    function _addPanel(title, content, panel, showCopy) {
        var div = jq('<div class="doc_panel"></div>').css({ margin: 20, marginLeft: 10 }).appendTo(panel);
        var titlePanel = jq('<div class="doc_panel_title"></div>').innerText(title).css({ padding: 5 }).appendTo(div);
        var contentPanel = jq('<div class="doc_panel_content"></div>').innerText(content).css({ padding: 5 }).appendTo(div);

        if (showCopy) {
            jq('<a class="copy_button" href="javascript:void(0)" style="float:right;">&lt;Copy&gt;</a>').click(function () {
                clipboardData.setData('Text', contentPanel.innerText());
            }).appendTo(titlePanel);
        }

        return div;
    }

    function _addPanel2(title, content, panel, showCopy) {
        var div = jq('<div class="doc_panel_2"></div>').css({ margin: 20, marginLeft: 10, marginRight: 0 }).appendTo(panel);
        var titlePanel = jq('<div class="doc_panel_title_2"></div>').innerText(title).css({ padding: 5 }).appendTo(div);
        var contentPanel = jq('<div class="doc_panel_content_2"></div>').innerText(content).css({ padding: 5 }).appendTo(div);

        if (showCopy) {
            jq('<a class="copy_button_2" href="javascript:void(0)" style="float:right;">&lt;Copy&gt;</a>').click(function () {
                clipboardData.setData('Text', contentPanel.innerText());
            }).appendTo(titlePanel);
        }

        return div;
    }

    // 显示参数文档
    function _showArg(title, arg, panel, serviceName) {
        var p = jq('.doc_panel_content', _addPanel(title, '', panel));
        if (arg == null) {
            p.text('(无)');
            return;
        }

        // 显示参数描述
        var doc = arg.DocTree;
        if (doc != null) {
            function _showType(t, c) {
                jq(t.FieldDocs).each(function () {
                    _showField(this, c);
                });
            }

            function _getType(fullName) {
                for (var k = 0; k < types.length; k++) {
                    if (types[k].TypeFullName == fullName) return types[k];
                }

                return null;
            }

            function _showField(fd, c) {
                var shortName = fd.TypeShortName, fullName = fd.TypeFullName, name = fd.Name, summary = fd.Summary + '', remarks = fd.Remarks + '', pvs = fd.PossibleValues;
                if (pvs != null && pvs.length > 0) {
                    if (remarks.length > 0) remarks += '\r\n';
                    remarks += '◆可选值：\r\n　◇' + jq(pvs).select(function () {
                        return this.Desc == null || this.Desc == '' ? this.Value : this.Value + '：' + this.Desc;
                    }).join('\r\n　◇');
                }

                var c0 = jq('<div class="doc_field"><span class="name"></span> &lt;<span class="type"></span>&gt;'
                    + '<span class="summary"></span></div>').attr('title', remarks).appendTo(c);

                jq('.name', c0).text(name);
                jq('.type', c0).text(shortName);
                if (summary.length > 0) jq('.summary', c0).text('  ' + summary);
                if (remarks.length > 0) {
                    var remarkNode = jq('<div class="remarks" style="padding-left:20px;padding-top:2px;padding-botton:3px;display:none;"></div>').appendTo(c0).innerText(remarks);
                    jq('.name', c0).toggle(function () { remarkNode.show(); c0.attr('title', ''); }, function () { remarkNode.hide(); c0.attr('title', remarks); }).css('cursor', 'pointer');
                    jq('.name', c0).append('<span class="more" style="float:right" title="展开/折叠">...</span>');
                }

                if (_getElementShortName(shortName) == 'object') {
                    var t = _getType(fullName);
                    if (t != null) {
                        _showType(t, jq('<div></div>').appendTo(c).css({ paddingLeft: 20 }));
                    } else {
                        //alert(fullName + '未找到');
                    }
                }
            }

            function _getElementShortName(shortName) {
                var k = shortName.indexOf('[');
                return k < 0 ? shortName : shortName.substring(0, k);
            }

            var root = doc.Root, types = doc.SubTypeDocs;
            var p2 = jq('.doc_panel_content_2', _addPanel2('参数', '', p, true));
            _showType(root, p2);
        }

        // 显示示例文档
        if (arg.Samples != null) {
            jq(arg.Samples).each(function () {
                var sample = this;
                _addPanel2('示例 （' + sample.Format + '）', sample.Sample, p, true);
            });
        }

        // 代码生成
        if (!arg.IsEmpty) {
            var codePanel = jq('.doc_panel_content_2', _addPanel2('代码', '', p));
            jq(['Object-C', 'Java']).each(function () {
                var codeName = this + '', panel;
                var node = jq('<div class="doc_panel_content_code_frame"></div>').appendTo(codePanel).text(codeName);
                (panel = jq('<div class="doc_panel_content_code"><a href="javascript:void(0)">点击展开 &gt;&gt;</a></div>')).afterTo(node).click(function _click() {
                    panel.unbind('click', _click);
                    panel.innerText(jq.generateCode({ types: doc.SubTypeDocs }, codeName, { genImpl: true, genIntf: true }));

                    jq('<a class="copy_button_code" href="javascript:void(0)" style="float:right;">&lt;Copy&gt;</a>').click(function () {
                        clipboardData.setData('Text', panel.innerText());
                    }).appendTo(node);
                });
            });
        }
    }

    function _nil(s, s0) {
        if (s == null) return s0;
        if (typeof (s) != 'string') return s;
        if (s.length == 0) return s0;
        return s;
    }

    function _post(url, data, callback) {
        jq.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: callback,
            error: function (errObj) {
                var json = $.parseJSON(errObj.responseText);
                alert(json.msg);

            }
        });
    }

})(jQuery);