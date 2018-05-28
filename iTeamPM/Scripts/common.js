const $requestPost = function (form, action, done, failed, always, progress) {
    $.ajax({
        url: action,
        type: 'POST',
        headers: {
            'X-Post-Back-Token': $('base').first().attr('postback') || ''
        },
        data: form,
        async: true,
        cache: false,
        contentType: false,
        processData: false,
        success: (data) => {
            done(data);
        },
        error: (jqXHR, textStatus, errorThrown) => {
            failed("error " + jqXHR.status + " " + errorThrown);
        },
        complete: () => {
            if (always) always();
        },
        xhr: () => {
            let xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", (evt) => {
                if (evt.lengthComputable) {
                    let percentComplete = parseInt(evt.loaded / evt.total);
                    //Do something with upload progress here
                    if (percentComplete <= 25) {
                        percentComplete = 25
                    } else if (percentComplete > 25 && percentComplete <= 50) {
                        percentComplete = 50
                    } else if (percentComplete < 100) {
                        percentComplete = 75
                    } else {
                        percentComplete = 100
                    }
                    if (progress) {
                        progress(percentComplete)
                    }
                }
            }, false);

            xhr.addEventListener("progress", (evt) => {
                if (evt.lengthComputable) {
                    let percentComplete = parseInt(evt.loaded / evt.total);
                    //Do something with download progress
                    if (percentComplete <= 25) {
                        percentComplete = 25
                    } else if (percentComplete > 25 && percentComplete <= 50) {
                        percentComplete = 50
                    } else if (percentComplete < 100) {
                        percentComplete = 75
                    } else {
                        percentComplete = 100
                    }
                    if (progress) {
                        progress(percentComplete)
                    }
                }
            }, false);

            xhr.upload.addEventListener('loadend', function (e) {
                // When the request has completed (either in success or failure).
                // Just like 'load', even if the server hasn't 
                // responded that it finished processing the request.
                if (progress) {
                    progress(100)
                }
            });

            xhr.addEventListener('loadend', function (e) {
                // When the request has completed (either in success or failure).
                // Just like 'load', even if the server hasn't 
                // responded that it finished processing the request.
                if (progress) {
                    progress(100)
                }
            });
            return xhr;
        },
    });
}

const $requestGet = function (action, done, failed, always, progress) {
    $.ajax({
        url: action,
        type: 'GET',
        data: null,
        async: true,
        cache: false,
        contentType: false,
        processData: false,
        success: (data) => {
            done(data);
        },
        error: (jqXHR, textStatus, errorThrown) => {
            failed("error " + jqXHR.status + " " + errorThrown);
        },
        complete: () => {
            if (always) {
                always();
            }
        },
        xhr: () => {
            let xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", (evt) => {
                if (evt.lengthComputable) {
                    let percentComplete = parseInt(evt.loaded / evt.total);
                    //Do something with upload progress here
                    if (percentComplete <= 25) {
                        percentComplete = 25
                    } else if (percentComplete > 25 && percentComplete <= 50) {
                        percentComplete = 50
                    } else if (percentComplete < 100) {
                        percentComplete = 75
                    } else {
                        percentComplete = 100
                    }
                    if (progress) {
                        progress(percentComplete)
                    }
                }
            }, false);

            xhr.addEventListener("progress", (evt) => {
                if (evt.lengthComputable) {
                    let percentComplete = parseInt(evt.loaded / evt.total);
                    //Do something with download progress
                    if (percentComplete <= 25) {
                        percentComplete = 25
                    } else if (percentComplete > 25 && percentComplete <= 50) {
                        percentComplete = 50
                    } else if (percentComplete < 100) {
                        percentComplete = 75
                    } else {
                        percentComplete = 100
                    }
                    if (progress) {
                        progress(percentComplete)
                    }
                }
            }, false);
            xhr.upload.addEventListener('loadend', function (e) {
                // When the request has completed (either in success or failure).
                // Just like 'load', even if the server hasn't 
                // responded that it finished processing the request.
                if (progress) {
                    progress(100)
                }
            });

            xhr.addEventListener('loadend', function (e) {
                // When the request has completed (either in success or failure).
                // Just like 'load', even if the server hasn't 
                // responded that it finished processing the request.
                if (progress) {
                    progress(100)
                }
            });
            return xhr;
        },
    });
}

const $queryString = (a => {
    if (a == '') return {};
    let b = {};
    for (let i = 0; i < a.length; ++i) {
        let p = a[i].split('=', 2);
        b[p[0]] = (p.length == 1) ? '' : decodeURIComponent(p[1].replace(/\+/g, ' '));
    }
    return b;
})(window.location.search.substr(1).split('&'));

const $$get = $requestGet;
const $$post = $requestPost;
const $$param = $queryString;

Math.round2 = function (d, place) {
    place = place || 0;
    return place != 0 ? parseFloat(d.toFixed(place)) : parseInt(d.toFixed(place));
};
String.prototype.urlEncode = function () {
    let t = this;
    return encodeURIComponent(t);
};
String.isNullOrEmpty = function (t) {
    return t == '' || t == null;
};
Date.tryParse = function (t) {
    t = t || '';
    if (!isNaN(Date.parse(t))) {
        return new Date(t);
    }
    else {
        return null;
    }
};
Number.prototype.pad = function (size) {
    let s = String(this);
    while (s.length < (size || 2)) {
        s = '0' + s;
    }
    return s;
};
Number.prototype.formatN = function (place) {
    let s = place == null ? this.toString() : this.toFixed(place);
    let o = '';
    let sp = s.split('.');
    o = parseInt(sp[0]).toLocaleString('en-US');
    if (place == 0) return o;
    else {
        if (sp.length > 1) {
            o += '.' + sp[1];
        }
        return o;
    }
};
Date.thaiParse = function (t) {
    t = t || '';
    let a = t.split('/');
    if (a.length != 3)
        return null;
    let y = parseInt(a[2]);
    let M = parseInt(a[1]);
    let d = parseInt(a[0]);
    let o = new Date().getTimezoneOffset() * (-1);
    //2017-05-17T00:00:00+07:00
    let d_ = `${y.pad(4)}-${M.pad(2)}-${d.pad(2)}T00:00:00${o < 0 ? '-' : '+'}${(Math.floor(o / 60)).pad(2)}:${(o % 60).pad(2)}`;
    console.log(d_);
    return Date.tryParse(d_);
};
Date.diffDays = function (start, end) {
    return Math.ceil((end.getTime() - start.getTime()) / (1000 * 3600 * 24));
};
Date.prototype.format = function (format) {
    let self = this;
    let half = false;

    if (format && format[0] === '!') {
        half = true;
        format = format.substring(1);
    }

    if (format === undefined || format === null || format === '')
        return self.getFullYear() + '-' + (self.getMonth() + 1).toString().padLeft(2, '0') + '-' + self.getDate().toString().padLeft(2, '0') + 'T' + self.getHours().toString().padLeft(2, '0') + ':' + self.getMinutes().toString().padLeft(2, '0') + ':' + self.getSeconds().toString().padLeft(2, '0') + '.' + self.getMilliseconds().toString().padLeft(3, '0') + 'Z';

    let h = self.getHours();

    if (half) {
        if (h >= 12)
            h -= 12;
    }

    return format.replace(regexpDATEFORMAT, function (key) {
        switch (key) {
            case 'yyyy':
                return self.getFullYear();
            case 'yy':
                return self.getYear();
            case 'MM':
                return (self.getMonth() + 1).toString().padLeft(2, '0');
            case 'M':
                return (self.getMonth() + 1);
            case 'dd':
                return self.getDate().toString().padLeft(2, '0');
            case 'd':
                return self.getDate();
            case 'HH':
            case 'hh':
                return h.toString().padLeft(2, '0');
            case 'H':
            case 'h':
                return self.getHours();
            case 'mm':
                return self.getMinutes().toString().padLeft(2, '0');
            case 'm':
                return self.getMinutes();
            case 'ss':
                return self.getSeconds().toString().padLeft(2, '0');
            case 's':
                return self.getSeconds();
            case 'w':
            case 'ww':
                let tmp = new Date(+self);
                tmp.setHours(0, 0, 0);
                tmp.setDate(tmp.getDate() + 4 - (tmp.getDay() || 7));
                tmp = Math.ceil((((tmp - new Date(tmp.getFullYear(), 0, 1)) / 8.64e7) + 1) / 7);
                if (key === 'ww')
                    return tmp.toString().padLeft(2, '0');
                return tmp;
            case 'a':
                let a = 'AM';
                if (self.getHours() >= 12)
                    a = 'PM';
                return a;
        }
    });
};

//$(document).find("[number-format]").each(function (index, element) {
//    $(element).css("text-align", "right");
//});

const baseUrl = $('base').first().attr('href');

const $$cookies = {
    set: function (cname, cvalue, exdays) {
        let d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    },
    get: function (cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
}

class Pagination {
    constructor() {
        this.total_item = 0;
        this.skip = 0;
        this.take = 20;
        this.current_page = 1;
    }

    setItemsPerPage(x) {
        this.take = x;
    }

    getItemsPerPage() {
        return this.take || 0;
    }

    setTotalItems(x) {
        this.total_item = x;
    }

    getTotalItems() {
        return this.total_item || 0;
    }

    setCurrentPage(x) {
        this.current_page = x;
    }

    getCurrentPage() {
        return this.current_page;
    }

    getTotalPages() {
        let p = (this.total_item % this.take != 0) ? Math.floor(this.total_item / this.take) + 1 : Math.floor(this.total_item / this.take);
        p = p < 1 ? 1 : p;
        return p;
    }

    skipItems() {
        return ((this.current_page > 0 ? this.current_page : 1) - 1) * this.take;
    }

    getItemNo(index) {
        return (index + (this.getItemsPerPage() * (this.getCurrentPage() - 1)) + 1);
    }

    createPagesArray() {
        let total_pages = this.getTotalPages();
        let outp_arr = [];
        if (total_pages <= 7) {
            for (let i = 1; i <= total_pages; i++) {
                outp_arr.push({ page_no: i, active: i == this.current_page });
            }
        }
        else if (total_pages > 7 && this.current_page <= 4) {
            for (let i = 1; i <= 7; i++) {
                if (i == 7) {
                    //outp_arr.push({ page_no: 0, active: false, icon: null });
                    outp_arr.push({ page_no: total_pages, active: total_pages == this.current_page, icon: null });
                } else {
                    outp_arr.push({ page_no: i, active: i == this.current_page, icon: null });
                }
            }
        }
        else if (total_pages > 7 && this.current_page >= total_pages - 3) {
            for (let i = 1; i <= 7; i++) {
                if (i == 1) {
                    outp_arr.push({ page_no: 1, active: 1 == this.current_page, icon: null });
                    //outp_arr.push({ page_no: 0, active: false, icon: null });
                }
                else {
                    outp_arr.push({ page_no: total_pages - 7 + i, active: (total_pages - 7 + i) == this.current_page, icon: null });
                }
            }
        } else {
            let start = this.current_page - 7 + 4;
            let end = 0;
            for (let i = 1; i <= 7; i++) {
                if (i == 1) {
                    outp_arr.push({ page_no: 1, active: 1 == this.current_page, icon: null });
                    //outp_arr.push({ page_no: 0, active: false, icon: null });
                }
                else if (i == 7) {
                    //outp_arr.push({ page_no: 0, active: false, icon: null });
                    outp_arr.push({ page_no: total_pages, active: total_pages == this.current_page, icon: null });
                } else {
                    outp_arr.push({ page_no: start - 1 + i, active: (start - 1 + i) == this.current_page, icon: null });
                }
            }
        }
        return outp_arr;
    }
}
class CenterFunction {
    constructor() {

    }
    f_find_project_name(project, callback) {
        var url = "CenterData/f_find_project_name?project=" + project;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
    f_depart_name(dpt_code, callback) {
        var url = "CenterData/f_depart_name?dpt_code=" + dpt_code;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
    f_find_acct_name(acct_no, callback) {
        var url = "CenterData/f_find_acct_name?acct_no=" + acct_no;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
    f_find_fatyname(fatypecode, callback) {
        var url = "CenterData/f_find_fatyname?ps_code=" + fatypecode;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
    f_find_unit_name(unitcode, callback) {
        var url = "CenterData/f_find_unit_name?ps_unitcode=" + unitcode;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
    f_job_name(arg_jobcode, callback) {
        var url = "CenterData/f_job_name?arg_jobcode=" + arg_jobcode;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
    f_find_project_depart(project, dpt_code, callback) {
        var url = "CenterData/f_find_project_depart?project=" + project + '&dpt_code=' + dpt_code;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
    f_find_cust_type_code(ps_code, callback) {
        var url = "CenterData/f_find_cust_type_code?ps_code=" + ps_code;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
    f_bank_name(bank_id, callback) {
        var url = "CenterData/f_bank_name?bank_id=" + bank_id;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }

    f_bank_branch_name(bank_id, branch_id, callback) {
        var url = "CenterData/f_bank_branch_name?bank_id=" + bank_id + '&branch_id=' + branch_id;
        $_get(url, (d) => {

            if (callback) {
                callback(d.data);
            }
            //return d;
        });
    }
}