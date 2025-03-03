﻿// Warn if overriding existing method
if (Array.prototype.equals)
    console.warn("Overriding existing Array.prototype.equals. Possible causes: New API defines the method, there's a framework conflict or you've got double inclusions in your code.");
// attach the .equals method to Array's prototype to call it on any array
Array.prototype.equals = function (array) {
    // if the other array is a falsy value, return
    if (!array)
        return false;

    // compare lengths - can save a lot of time 
    if (this.length != array.length)
        return false;

    for (var i = 0, l = this.length; i < l; i++) {
        // Check if we have nested arrays
        if (this[i] instanceof Array && array[i] instanceof Array) {
            // recurse into the nested arrays
            if (!this[i].equals(array[i]))
                return false;
        }
        else if (this[i] != array[i]) {
            // Warning - two different object instances will never be equal: {x:20} != {x:20}
            return false;
        }
    }
    return true;
}
// Hide method from for-in loops
Object.defineProperty(Array.prototype, "equals", { enumerable: false });

Array.prototype.removeByVal = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === val) {
            this.splice(i, 1);
            i--;
        }
    }
    return this;
}


$.extend(
{
    redirectPost: function(location, args)
    {
        var form = '';
        $.each( args, function( key, value ) {
            value = value.split('"').join('\"')
            form += '<input type="hidden" name="'+key+'" value="'+value+'">';
        });
        $('<form action="' + location + '" method="POST">' + form + '</form>').appendTo($(document.body)).submit();
    }
    });

(function ($) {

    $.fn.fixedHeader = function (options) {
        var config = {
            topOffset: 50
            //bgColor: 'white'
        };
        if (options) {
            $.extend(config, options);
        }

        return this.each(function () {
            var o = $(this);
            var o = $(this);

            var $win = $(window);
            var $head = $('thead.header', o);
            var isFixed = 0;
            var headTop = $head.length && $head.offset().top - config.topOffset;

            function processScroll() {
                if (!o.is(':visible')) {
                    return;
                }
                if ($('thead.header-copy').size()) {
                    $('thead.header-copy').width($('thead.header').width());
                }
                var i;
                var scrollTop = $win.scrollTop();
                var t = $head.length && $head.offset().top - config.topOffset;
                if (!isFixed && headTop !== t) {
                    headTop = t;
                }
                if (scrollTop >= headTop && !isFixed) {
                    isFixed = 1;
                } else if (scrollTop <= headTop && isFixed) {
                    isFixed = 0;
                }
                isFixed ? $('thead.header-copy', o).offset({
                    left: $head.offset().left
                }).removeClass('hide') : $('thead.header-copy', o).addClass('hide');
            }
            $win.on('scroll', processScroll);

            // hack sad times - holdover until rewrite for 2.1
            $head.on('click', function () {
                if (!isFixed) {
                    setTimeout(function () {
                        $win.scrollTop($win.scrollTop() - 47);
                    }, 10);
                }
            });

            $head.clone().removeClass('header').addClass('header-copy header-fixed').appendTo(o);
            var header_width = $head.width();
            o.find('thead.header-copy').width(header_width);
            o.find('thead.header > tr:first > th').each(function (i, h) {
                var w = $(h).width();
                o.find('thead.header-copy> tr > th:eq(' + i + ')').width(w);
            });
            $head.css({
                margin: '0 auto',
                width: o.width(),
                'background-color': config.bgColor
            });
            processScroll();
        });
    };

})(jQuery);