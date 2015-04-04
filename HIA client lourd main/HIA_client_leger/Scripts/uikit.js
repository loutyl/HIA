/*! UIkit 2.18.0 | http://www.getuikit.com | (c) 2014 YOOtheme | MIT License */
(function(core) {

    if (typeof define == "function" && define.amd) { // AMD
        define("uikit", function(){

            var uikit = window.UIkit || core(window, window.jQuery, window.document);

            uikit.load = function(res, req, onload, config) {

                var resources = res.split(','), load = [], i, base = (config.config && config.config.uikit && config.config.uikit.base ? config.config.uikit.base : "").replace(/\/+$/g, "");

                if (!base) {
                    throw new Error( "Please define base path to UIkit in the requirejs config." );
                }

                for (i = 0; i < resources.length; i += 1) {
                    var resource = resources[i].replace(/\./g, '/');
                    load.push(base+'/components/'+resource);
                }

                req(load, function() {
                    onload(uikit);
                });
            };

            return uikit;
        });
    }

    if (!window.jQuery) {
        throw new Error( "UIkit requires jQuery" );
    }

    if (window && window.jQuery) {
        core(window, window.jQuery, window.document);
    }


})(function(global, $, doc) {

    "use strict";

    var ui = {}, ui = window.UIkit;

    ui.version = '2.18.0';

    ui.noConflict = function() {
        // resore UIkit version
        if (ui) {
            window.UIkit = ui;
            $.UIkit      = ui;
            $.fn.uk      = ui.fn;
        }

        return ui;
    };

    ui.prefix = function(str) {
        return str;
    };

    // cache jQuery
    ui.$ = $;

    ui.$doc  = ui.$(document);
    ui.$win  = ui.$(window);
    ui.$html = ui.$('html');

    ui.fn = function(command, options) {

        var args = arguments, cmd = command.match(/^([a-z\-]+)(?:\.([a-z]+))?/i), component = cmd[1], method = cmd[2];

        if (!ui[component]) {
            $.error("UIkit component [" + component + "] does not exist.");
            return this;
        }

        return this.each(function() {
            var $this = $(this), data = $this.data(component);
            if (!data) $this.data(component, (data = ui[component](this, method ? undefined : options)));
            if (method) data[method].apply(data, Array.prototype.slice.call(args, 1));
        });
    };

    ui.support = {};
    ui.support.transition = (function() {

        var transitionEnd = (function() {

            var element = doc.body || doc.documentElement,
                transEndEventNames = {
                    WebkitTransition : 'webkitTransitionEnd',
                    MozTransition    : 'transitionend',
                    OTransition      : 'oTransitionEnd otransitionend',
                    transition       : 'transitionend'
                }, name;

            for (name in transEndEventNames) {
                if (element.style[name] !== undefined) return transEndEventNames[name];
            }
        }());

        return transitionEnd && { end: transitionEnd };
    })();

    ui.support.animation = (function() {

        var animationEnd = (function() {

            var element = doc.body || doc.documentElement,
                animEndEventNames = {
                    WebkitAnimation : 'webkitAnimationEnd',
                    MozAnimation    : 'animationend',
                    OAnimation      : 'oAnimationEnd oanimationend',
                    animation       : 'animationend'
                }, name;

            for (name in animEndEventNames) {
                if (element.style[name] !== undefined) return animEndEventNames[name];
            }
        }());

        return animationEnd && { end: animationEnd };
    })();

    ui.support.requestAnimationFrame = window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame || window.msRequestAnimationFrame || window.oRequestAnimationFrame || function(callback){ setTimeout(callback, 1000/60); };
    ui.support.touch                 = (
        ('ontouchstart' in window && navigator.userAgent.toLowerCase().match(/mobile|tablet/)) ||
        (global.DocumentTouch && document instanceof global.DocumentTouch)  ||
        (global.navigator.msPointerEnabled && global.navigator.msMaxTouchPoints > 0) || //IE 10
        (global.navigator.pointerEnabled && global.navigator.maxTouchPoints > 0) || //IE >=11
        false
    );
    ui.support.mutationobserver = (global.MutationObserver || global.WebKitMutationObserver || null);

    ui.Utils = {};

    ui.Utils.str2json = function(str, notevil) {
        try {
            if (notevil) {
                return JSON.parse(str
                    // wrap keys without quote with valid double quote
                    .replace(/([\$\w]+)\s*:/g, function(_, $1){return '"'+$1+'":';})
                    // replacing single quote wrapped ones to double quote
                    .replace(/'([^']+)'/g, function(_, $1){return '"'+$1+'"';})
                );
            } else {
                return (new Function("", "var json = " + str + "; return JSON.parse(JSON.stringify(json));"))();
            }
        } catch(e) { return false; }
    };

    ui.Utils.debounce = function(func, wait, immediate) {
        var timeout;
        return function() {
            var context = this, args = arguments;
            var later = function() {
                timeout = null;
                if (!immediate) func.apply(context, args);
            };
            var callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow) func.apply(context, args);
        };
    };

    ui.Utils.removeCssRules = function(selectorRegEx) {
        var idx, idxs, stylesheet, i, j, k, len, len1, len2, ref;

        if(!selectorRegEx) return;

        setTimeout(function(){
            try {
              ref = document.styleSheets;
              for (i = 0, len = ref.length; i < len; i++) {
                stylesheet = ref[i];
                idxs = [];
                stylesheet.cssRules = stylesheet.cssRules;
                for (idx = j = 0, len1 = stylesheet.cssRules.length; j < len1; idx = ++j) {
                  if (stylesheet.cssRules[idx].type === CSSRule.STYLE_RULE && selectorRegEx.test(stylesheet.cssRules[idx].selectorText)) {
                    idxs.unshift(idx);
                  }
                }
                for (k = 0, len2 = idxs.length; k < len2; k++) {
                  stylesheet.deleteRule(idxs[k]);
                }
              }
            } catch (error) {}
        }, 0);
    };

    ui.Utils.isInView = function(element, options) {

        var $element = $(element);

        if (!$element.is(':visible')) {
            return false;
        }

        var windowLeft = ui.$win.scrollLeft(), windowTop = ui.$win.scrollTop(), offset = $element.offset(), left = offset.left, top = offset.top;

        options = $.extend({topoffset:0, leftoffset:0}, options);

        if (top + $element.height() >= windowTop && top - options.topoffset <= windowTop + ui.$win.height() &&
            left + $element.width() >= windowLeft && left - options.leftoffset <= windowLeft + ui.$win.width()) {
          return true;
        } else {
          return false;
        }
    };

    ui.Utils.checkDisplay = function(context, initanimation) {

        var elements = ui.$('[data-uk-margin], [data-uk-grid-match], [data-uk-grid-margin], [data-uk-check-display]', context || document), animated;

        if (context && !elements.length) {
            elements = $(context);
        }

        elements.trigger('display.uk.check');

        // fix firefox / IE animations
        if (initanimation) {

            if (typeof(initanimation)!='string') {
                initanimation = '[class*="uk-animation-"]';
            }

            elements.find(initanimation).each(function(){

                var ele  = ui.$(this),
                    cls  = ele.attr('class'),
                    anim = cls.match(/uk\-animation\-(.+)/);

                ele.removeClass(anim[0]).width();

                ele.addClass(anim[0]);
            });
        }

        return elements;
    };

    ui.Utils.options = function(string) {

        if ($.isPlainObject(string)) return string;

        var start = (string ? string.indexOf("{") : -1), options = {};

        if (start != -1) {
            try {
                options = ui.Utils.str2json(string.substr(start));
            } catch (e) {}
        }

        return options;
    };

    ui.Utils.animate = function(element, cls) {

        var d = $.Deferred();

        element = ui.$(element);
        cls     = cls;

        element.css('display', 'none').addClass(cls).one(ui.support.animation.end, function() {
            element.removeClass(cls);
            d.resolve();
        }).width();

        element.css('display', '');

        return d.promise();
    };

    ui.Utils.uid = function(prefix) {
        return (prefix || 'id') + (new Date().getTime())+"RAND"+(Math.ceil(Math.random() * 100000));
    };

    ui.Utils.template = function(str, data) {

        var tokens = str.replace(/\n/g, '\\n').replace(/\{\{\{\s*(.+?)\s*\}\}\}/g, "{{!$1}}").split(/(\{\{\s*(.+?)\s*\}\})/g),
            i=0, toc, cmd, prop, val, fn, output = [], openblocks = 0;

        while(i < tokens.length) {

            toc = tokens[i];

            if(toc.match(/\{\{\s*(.+?)\s*\}\}/)) {
                i = i + 1;
                toc  = tokens[i];
                cmd  = toc[0];
                prop = toc.substring(toc.match(/^(\^|\#|\!|\~|\:)/) ? 1:0);

                switch(cmd) {
                    case '~':
                        output.push("for(var $i=0;$i<"+prop+".length;$i++) { var $item = "+prop+"[$i];");
                        openblocks++;
                        break;
                    case ':':
                        output.push("for(var $key in "+prop+") { var $val = "+prop+"[$key];");
                        openblocks++;
                        break;
                    case '#':
                        output.push("if("+prop+") {");
                        openblocks++;
                        break;
                    case '^':
                        output.push("if(!"+prop+") {");
                        openblocks++;
                        break;
                    case '/':
                        output.push("}");
                        openblocks--;
                        break;
                    case '!':
                        output.push("__ret.push("+prop+");");
                        break;
                    default:
                        output.push("__ret.push(escape("+prop+"));");
                        break;
                }
            } else {
                output.push("__ret.push('"+toc.replace(/\'/g, "\\'")+"');");
            }
            i = i + 1;
        }

        fn  = new Function('$data', [
            'var __ret = [];',
            'try {',
            'with($data){', (!openblocks ? output.join('') : '__ret = ["Not all blocks are closed correctly."]'), '};',
            '}catch(e){__ret = [e.message];}',
            'return __ret.join("").replace(/\\n\\n/g, "\\n");',
            "function escape(html) { return String(html).replace(/&/g, '&amp;').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');}"
        ].join("\n"));

        return data ? fn(data) : fn;
    };

    ui.Utils.events       = {};
    ui.Utils.events.click = ui.support.touch ? 'tap' : 'click';

    window.UIkit = ui;
    $.UIkit      = ui;
    $.fn.uk      = ui.fn;

    ui.langdirection = ui.$html.attr("dir") == "rtl" ? "right" : "left";

    ui.components = {};

    ui.component = function(name, def) {

        var fn = function(element, options) {

            var $this = this;

            this.UIkit   = ui;
            this.element = element ? ui.$(element) : null;
            this.options = $.extend(true, {}, this.defaults, options);
            this.plugins = {};

            if (this.element) {
                this.element.data(name, this);
            }

            this.init();

            (this.options.plugins.length ? this.options.plugins : Object.keys(fn.plugins)).forEach(function(plugin) {

                if (fn.plugins[plugin].init) {
                    fn.plugins[plugin].init($this);
                    $this.plugins[plugin] = true;
                }

            });

            this.trigger('init.uk.component', [name, this]);

            return this;
        };

        fn.plugins = {};

        $.extend(true, fn.prototype, {

            defaults : {plugins: []},

            boot: function(){},
            init: function(){},

            on: function(a1,a2,a3){
                return ui.$(this.element || this).on(a1,a2,a3);
            },

            one: function(a1,a2,a3){
                return ui.$(this.element || this).one(a1,a2,a3);
            },

            off: function(evt){
                return ui.$(this.element || this).off(evt);
            },

            trigger: function(evt, params) {
                return ui.$(this.element || this).trigger(evt, params);
            },

            find: function(selector) {
                return ui.$(this.element ? this.element: []).find(selector);
            },

            proxy: function(obj, methods) {

                var $this = this;

                methods.split(' ').forEach(function(method) {
                    if (!$this[method]) $this[method] = function() { return obj[method].apply(obj, arguments); };
                });
            },

            mixin: function(obj, methods) {

                var $this = this;

                methods.split(' ').forEach(function(method) {
                    if (!$this[method]) $this[method] = obj[method].bind($this);
                });
            },

            option: function() {

                if (arguments.length == 1) {
                    return this.options[arguments[0]] || undefined;
                } else if (arguments.length == 2) {
                    this.options[arguments[0]] = arguments[1];
                }
            }

        }, def);

        this.components[name] = fn;

        this[name] = function() {

            var element, options;

            if (arguments.length) {

                switch(arguments.length) {
                    case 1:

                        if (typeof arguments[0] === "string" || arguments[0].nodeType || arguments[0] instanceof jQuery) {
                            element = $(arguments[0]);
                        } else {
                            options = arguments[0];
                        }

                        break;
                    case 2:

                        element = $(arguments[0]);
                        options = arguments[1];
                        break;
                }
            }

            if (element && element.data(name)) {
                return element.data(name);
            }

            return (new ui.components[name](element, options));
        };

        if (ui.domready) {
            ui.component.boot(name);
        }

        return fn;
    };

    ui.plugin = function(component, name, def) {
        this.components[component].plugins[name] = def;
    };

    ui.component.boot = function(name) {

        if (ui.components[name].prototype && ui.components[name].prototype.boot && !ui.components[name].booted) {
            ui.components[name].prototype.boot.apply(ui, []);
            ui.components[name].booted = true;
        }
    };

    ui.component.bootComponents = function() {

        for (var component in ui.components) {
            ui.component.boot(component);
        }
    };


    // DOM mutation save ready helper function

    ui.domObservers = [];
    ui.domready     = false;

    ui.ready = function(fn) {

        ui.domObservers.push(fn);

        if (ui.domready) {
            fn(document);
        }
    };

    ui.on = function(a1,a2,a3){

        if (a1 && a1.indexOf('ready.uk.dom') > -1 && ui.domready) {
            a2.apply(ui.$doc);
        }

        return ui.$doc.on(a1,a2,a3);
    };

    ui.one = function(a1,a2,a3){

        if (a1 && a1.indexOf('ready.uk.dom') > -1 && ui.domready) {
            a2.apply(ui.$doc);
            return ui.$doc;
        }

        return ui.$doc.one(a1,a2,a3);
    };

    ui.trigger = function(evt, params) {
        return ui.$doc.trigger(evt, params);
    };

    ui.domObserve = function(selector, fn) {

        if(!ui.support.mutationobserver) return;

        fn = fn || function() {};

        ui.$(selector).each(function() {

            var element  = this,
                $element = ui.$(element);

            if ($element.data('observer')) {
                return;
            }

            try {

                var observer = new ui.support.mutationobserver(ui.Utils.debounce(function(mutations) {
                    fn.apply(element, []);
                    $element.trigger('changed.uk.dom');
                }, 50));

                // pass in the target node, as well as the observer options
                observer.observe(element, { childList: true, subtree: true });

                $element.data('observer', observer);

            } catch(e) {}
        });
    };

    ui.on('domready.uk.dom', function(){

        ui.domObservers.forEach(function(fn){
            fn(document);
        });

        if (ui.domready) ui.Utils.checkDisplay(document);
    });

    $(function(){

        ui.$body = ui.$('body');

        ui.ready(function(context){
            ui.domObserve('[data-uk-observe]');
        });

        ui.on('changed.uk.dom', function(e) {

            var ele = e.target;

            ui.domObservers.forEach(function(fn){
                fn(ele);
            });

            ui.Utils.checkDisplay(ele);
        });

        ui.trigger('beforeready.uk.dom');

        ui.component.bootComponents();

        // custom scroll observer
        setInterval((function(){

            var memory = {x: window.pageXOffset, y:window.pageYOffset}, dir;

            var fn = function(){

                if (memory.x != window.pageXOffset || memory.y != window.pageYOffset) {

                    dir = {x: 0 , y: 0};

                    if (window.pageXOffset != memory.x) dir.x = window.pageXOffset > memory.x ? 1:-1;
                    if (window.pageYOffset != memory.y) dir.y = window.pageYOffset > memory.y ? 1:-1;

                    memory = {
                        "dir": dir, "x": window.pageXOffset, "y": window.pageYOffset
                    };

                    ui.$doc.trigger('scrolling.uk.document', [memory]);
                }
            };

            if (ui.support.touch) {
                ui.$html.on('touchmove touchend MSPointerMove MSPointerUp pointermove pointerup', fn);
            }

            if (memory.x || memory.y) fn();

            return fn;

        })(), 15);

        // run component init functions on dom
        ui.trigger('domready.uk.dom');

        if (ui.support.touch) {

            // remove css hover rules for touch devices
            // UI.Utils.removeCssRules(/\.uk-(?!navbar).*:hover/);

            // viewport unit fix for uk-height-viewport - should be fixed in iOS 8
            if (navigator.userAgent.match(/(iPad|iPhone|iPod)/g)) {

                ui.$win.on('load orientationchange resize', ui.Utils.debounce((function(){

                    var fn = function() {
                        $('.uk-height-viewport').css('height', window.innerHeight);
                        return fn;
                    };

                    return fn();

                })(), 100));
            }
        }

        ui.trigger('afterready.uk.dom');

        // mark that domready is left behind
        ui.domready = true;
    });

    // add touch identifier class
    ui.$html.addClass(ui.support.touch ? "uk-touch" : "uk-notouch");

    // add uk-hover class on tap to support overlays on touch devices
    if (ui.support.touch) {

        var hoverset = false, exclude, hovercls = 'uk-hover', selector = '.uk-overlay, .uk-overlay-hover, .uk-overlay-toggle, .uk-animation-hover, .uk-has-hover';

        ui.$html.on('touchstart MSPointerDown pointerdown', selector, function() {

            if (hoverset) $('.'+hovercls).removeClass(hovercls);

            hoverset = $(this).addClass(hovercls);

        }).on('touchend MSPointerUp pointerup', function(e) {

            exclude = $(e.target).parents(selector);

            if (hoverset) {
                hoverset.not(exclude).removeClass(hovercls);
            }
        });
    }

    return ui;
});

//  Based on Zeptos touch.js
//  https://raw.github.com/madrobby/zepto/master/src/touch.js
//  Zepto.js may be freely distributed under the MIT license.

;(function($){

  if ($.fn.swipeLeft) {
    return;
  }


  var touch = {}, touchTimeout, tapTimeout, swipeTimeout, longTapTimeout, longTapDelay = 750, gesture;

  function swipeDirection(x1, x2, y1, y2) {
    return Math.abs(x1 - x2) >= Math.abs(y1 - y2) ? (x1 - x2 > 0 ? 'Left' : 'Right') : (y1 - y2 > 0 ? 'Up' : 'Down');
  }

  function longTap() {
    longTapTimeout = null;
    if (touch.last) {
      touch.el.trigger('longTap');
      touch = {};
    }
  }

  function cancelLongTap() {
    if (longTapTimeout) clearTimeout(longTapTimeout);
    longTapTimeout = null;
  }

  function cancelAll() {
    if (touchTimeout)   clearTimeout(touchTimeout);
    if (tapTimeout)     clearTimeout(tapTimeout);
    if (swipeTimeout)   clearTimeout(swipeTimeout);
    if (longTapTimeout) clearTimeout(longTapTimeout);
    touchTimeout = tapTimeout = swipeTimeout = longTapTimeout = null;
    touch = {};
  }

  function isPrimaryTouch(event){
    return event.pointerType == event.MSPOINTER_TYPE_TOUCH && event.isPrimary;
  }

  $(function(){
    var now, delta, deltaX = 0, deltaY = 0, firstTouch;

    if ('MSGesture' in window) {
      gesture = new MSGesture();
      gesture.target = document.body;
    }

    $(document)
      .on('MSGestureEnd gestureend', function(e){

        var swipeDirectionFromVelocity = e.originalEvent.velocityX > 1 ? 'Right' : e.originalEvent.velocityX < -1 ? 'Left' : e.originalEvent.velocityY > 1 ? 'Down' : e.originalEvent.velocityY < -1 ? 'Up' : null;

        if (swipeDirectionFromVelocity) {
          touch.el.trigger('swipe');
          touch.el.trigger('swipe'+ swipeDirectionFromVelocity);
        }
      })
      // MSPointerDown: for IE10
      // pointerdown: for IE11
      .on('touchstart MSPointerDown pointerdown', function(e){

        if(e.type == 'MSPointerDown' && !isPrimaryTouch(e.originalEvent)) return;

        firstTouch = (e.type == 'MSPointerDown' || e.type == 'pointerdown') ? e : e.originalEvent.touches[0];

        now      = Date.now();
        delta    = now - (touch.last || now);
        touch.el = $('tagName' in firstTouch.target ? firstTouch.target : firstTouch.target.parentNode);

        if(touchTimeout) clearTimeout(touchTimeout);

        touch.x1 = firstTouch.pageX;
        touch.y1 = firstTouch.pageY;

        if (delta > 0 && delta <= 250) touch.isDoubleTap = true;

        touch.last = now;
        longTapTimeout = setTimeout(longTap, longTapDelay);

        // adds the current touch contact for IE gesture recognition
        if (gesture && ( e.type == 'MSPointerDown' || e.type == 'pointerdown' || e.type == 'touchstart' ) ) {
          gesture.addPointer(e.originalEvent.pointerId);
        }

      })
      // MSPointerMove: for IE10
      // pointermove: for IE11
      .on('touchmove MSPointerMove pointermove', function(e){

        if (e.type == 'MSPointerMove' && !isPrimaryTouch(e.originalEvent)) return;

        firstTouch = (e.type == 'MSPointerMove' || e.type == 'pointermove') ? e : e.originalEvent.touches[0];

        cancelLongTap();
        touch.x2 = firstTouch.pageX;
        touch.y2 = firstTouch.pageY;

        deltaX += Math.abs(touch.x1 - touch.x2);
        deltaY += Math.abs(touch.y1 - touch.y2);
      })
      // MSPointerUp: for IE10
      // pointerup: for IE11
      .on('touchend MSPointerUp pointerup', function(e){

        if (e.type == 'MSPointerUp' && !isPrimaryTouch(e.originalEvent)) return;

        cancelLongTap();

        // swipe
        if ((touch.x2 && Math.abs(touch.x1 - touch.x2) > 30) || (touch.y2 && Math.abs(touch.y1 - touch.y2) > 30)){

          swipeTimeout = setTimeout(function() {
            touch.el.trigger('swipe');
            touch.el.trigger('swipe' + (swipeDirection(touch.x1, touch.x2, touch.y1, touch.y2)));
            touch = {};
          }, 0);

        // normal tap
        } else if ('last' in touch) {

          // don't fire tap when delta position changed by more than 30 pixels,
          // for instance when moving to a point and back to origin
          if (isNaN(deltaX) || (deltaX < 30 && deltaY < 30)) {
            // delay by one tick so we can cancel the 'tap' event if 'scroll' fires
            // ('tap' fires before 'scroll')
            tapTimeout = setTimeout(function() {

              // trigger universal 'tap' with the option to cancelTouch()
              // (cancelTouch cancels processing of single vs double taps for faster 'tap' response)
              var event = $.Event('tap');
              event.cancelTouch = cancelAll;
              touch.el.trigger(event);

              // trigger double tap immediately
              if (touch.isDoubleTap) {
                touch.el.trigger('doubleTap');
                touch = {};
              }

              // trigger single tap after 250ms of inactivity
              else {
                touchTimeout = setTimeout(function(){
                  touchTimeout = null;
                  touch.el.trigger('singleTap');
                  touch = {};
                }, 250);
              }
            }, 0);
          } else {
            touch = {};
          }
          deltaX = deltaY = 0;
        }
      })
      // when the browser window loses focus,
      // for example when a modal dialog is shown,
      // cancel all ongoing events
      .on('touchcancel MSPointerCancel', cancelAll);

    // scrolling the window indicates intention of the user
    // to scroll, not tap or swipe, so cancel all ongoing events
    $(window).on('scroll', cancelAll);
  });

  ['swipe', 'swipeLeft', 'swipeRight', 'swipeUp', 'swipeDown', 'doubleTap', 'tap', 'singleTap', 'longTap'].forEach(function(eventName){
    $.fn[eventName] = function(callback){ return $(this).on(eventName, callback); };
  });
})(jQuery);

(function(ui) {

    "use strict";

    var stacks = [];

    ui.component('stackMargin', {

        defaults: {
            'cls': 'uk-margin-small-top'
        },

        boot: function() {

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-margin]", context).each(function() {

                    var ele = ui.$(this), obj;

                    if (!ele.data("stackMargin")) {
                        obj = ui.stackMargin(ele, ui.Utils.options(ele.attr("data-uk-margin")));
                    }
                });
            });
        },

        init: function() {

            var $this = this;

            this.columns = this.element.children();

            if (!this.columns.length) return;

            ui.$win.on('resize orientationchange', (function() {

                var fn = function() {
                    $this.process();
                };

                ui.$(function() {
                    fn();
                    ui.$win.on("load", fn);
                });

                return ui.Utils.debounce(fn, 20);
            })());

            ui.$html.on("changed.uk.dom", function(e) {
                $this.columns  = $this.element.children();
                $this.process();
            });

            this.on("display.uk.check", function(e) {
                $this.columns = $this.element.children();
                if (this.element.is(":visible")) this.process();
            }.bind(this));

            stacks.push(this);
        },

        process: function() {

            var $this = this;

            ui.Utils.stackMargin(this.columns, this.options);

            return this;
        },

        revert: function() {
            this.columns.removeClass(this.options.cls);
            return this;
        }
    });

    // responsive iframes
    ui.ready((function(){

        var iframes = [], check = function() {

            iframes.forEach(function(iframe){

                if (!iframe.is(':visible')) return;

                var width  = iframe.parent().width(),
                    iwidth = iframe.data('width'),
                    ratio  = (width / iwidth),
                    height = Math.floor(ratio * iframe.data('height'));

                iframe.css({'height': (width < iwidth) ? height : iframe.data('height')});
            });
        };

        ui.$win.on('resize', ui.Utils.debounce(check, 15));

        return function(context){

            ui.$('iframe.uk-responsive-width', context).each(function(){

                var iframe = ui.$(this);

                if (!iframe.data('responsive') && iframe.attr('width') && iframe.attr('height')) {

                    iframe.data('width'     , iframe.attr('width'));
                    iframe.data('height'    , iframe.attr('height'));
                    iframe.data('responsive', true);
                    iframes.push(iframe);
                }
            });

            check();
        };

    })());


    // helper

    ui.Utils.stackMargin = function(elements, options) {

        options = ui.$.extend({
            'cls': 'uk-margin-small-top'
        }, options);

        options.cls = options.cls;

        elements = ui.$(elements).removeClass(options.cls);

        var skip         = false,
            firstvisible = elements.filter(":visible:first"),
            offset       = firstvisible.length ? (firstvisible.position().top + firstvisible.outerHeight()) - 1 : false; // (-1): weird firefox bug when parent container is display:flex

        if (offset === false) return;

        elements.each(function() {

            var column = ui.$(this);

            if (column.is(":visible")) {

                if (skip) {
                    column.addClass(options.cls);
                } else {

                    if (column.position().top >= offset) {
                        skip = column.addClass(options.cls);
                    }
                }
            }
        });
    };

})(UIkit);

(function(ui) {

    "use strict";

    ui.component('smoothScroll', {

        boot: function() {

            // init code
            ui.$html.on("click.smooth-scroll.uikit", "[data-uk-smooth-scroll]", function(e) {
                var ele = ui.$(this);

                if (!ele.data("smoothScroll")) {
                    var obj = ui.smoothScroll(ele, ui.Utils.options(ele.attr("data-uk-smooth-scroll")));
                    ele.trigger("click");
                }

                return false;
            });
        },

        init: function() {

            var $this = this;

            this.on("click", function(e) {
                e.preventDefault();
                scrollToElement(ui.$(this.hash).length ? ui.$(this.hash) : ui.$("body"), $this.options);
            });
        }
    });

    function scrollToElement(ele, options) {

        options = ui.$.extend({
            duration: 1000,
            transition: 'easeOutExpo',
            offset: 0,
            complete: function(){}
        }, options);

        // get / set parameters
        var target    = ele.offset().top - options.offset,
            docheight = ui.$doc.height(),
            winheight = window.innerHeight;

        if ((target + winheight) > docheight) {
            target = docheight - winheight;
        }

        // animate to target, fire callback when done
        ui.$("html,body").stop().animate({scrollTop: target}, options.duration, options.transition).promise().done(options.complete);
    }

    ui.Utils.scrollToElement = scrollToElement;

    if (!ui.$.easing.easeOutExpo) {
        ui.$.easing.easeOutExpo = function(x, t, b, c, d) { return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b; };
    }

})(UIkit);

(function(ui) {

    "use strict";

    var $win           = ui.$win,
        $doc           = ui.$doc,
        scrollspies    = [],
        checkScrollSpy = function() {
            for(var i=0; i < scrollspies.length; i++) {
                ui.support.requestAnimationFrame.apply(window, [scrollspies[i].check]);
            }
        };

    ui.component('scrollspy', {

        defaults: {
            "target"     : false,
            "cls"        : "uk-scrollspy-inview",
            "initcls"    : "uk-scrollspy-init-inview",
            "topoffset"  : 0,
            "leftoffset" : 0,
            "repeat"     : false,
            "delay"      : 0
        },

        boot: function() {

            // listen to scroll and resize
            $doc.on("scrolling.uk.document", checkScrollSpy);
            $win.on("load resize orientationchange", ui.Utils.debounce(checkScrollSpy, 50));

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-scrollspy]", context).each(function() {

                    var element = ui.$(this);

                    if (!element.data("scrollspy")) {
                        var obj = ui.scrollspy(element, ui.Utils.options(element.attr("data-uk-scrollspy")));
                    }
                });
            });
        },

        init: function() {

            var $this = this, inviewstate, initinview, togglecls = this.options.cls.split(/,/), fn = function(){

                var elements     = $this.options.target ? $this.element.find($this.options.target) : $this.element,
                    delayIdx     = elements.length === 1 ? 1 : 0,
                    toggleclsIdx = 0;

                elements.each(function(idx){

                    var element     = ui.$(this),
                        inviewstate = element.data('inviewstate'),
                        inview      = ui.Utils.isInView(element, $this.options),
                        toggle      = element.data('ukScrollspyCls') || togglecls[toggleclsIdx].trim();

                    if (inview && !inviewstate && !element.data('scrollspy-idle')) {

                        if (!initinview) {
                            element.addClass($this.options.initcls);
                            $this.offset = element.offset();
                            initinview = true;

                            element.trigger("init.uk.scrollspy");
                        }

                        element.data('scrollspy-idle', setTimeout(function(){

                            element.addClass("uk-scrollspy-inview").toggleClass(toggle).width();
                            element.trigger("inview.uk.scrollspy");

                            element.data('scrollspy-idle', false);
                            element.data('inviewstate', true);

                        }, $this.options.delay * delayIdx));

                        delayIdx++;
                    }

                    if (!inview && inviewstate && $this.options.repeat) {

                        if (element.data('scrollspy-idle')) {
                            clearTimeout(element.data('scrollspy-idle'));
                        }

                        element.removeClass("uk-scrollspy-inview").toggleClass(toggle);
                        element.data('inviewstate', false);

                        element.trigger("outview.uk.scrollspy");
                    }

                    toggleclsIdx = togglecls[toggleclsIdx + 1] ? (toggleclsIdx + 1) : 0;

                });
            };

            fn();

            this.check = fn;

            scrollspies.push(this);
        }
    });


    var scrollspynavs = [],
        checkScrollSpyNavs = function() {
            for(var i=0; i < scrollspynavs.length; i++) {
                ui.support.requestAnimationFrame.apply(window, [scrollspynavs[i].check]);
            }
        };

    ui.component('scrollspynav', {

        defaults: {
            "cls"          : 'uk-active',
            "closest"      : false,
            "topoffset"    : 0,
            "leftoffset"   : 0,
            "smoothscroll" : false
        },

        boot: function() {

            // listen to scroll and resize
            $doc.on("scrolling.uk.document", checkScrollSpyNavs);
            $win.on("resize orientationchange", ui.Utils.debounce(checkScrollSpyNavs, 50));

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-scrollspy-nav]", context).each(function() {

                    var element = ui.$(this);

                    if (!element.data("scrollspynav")) {
                        var obj = ui.scrollspynav(element, ui.Utils.options(element.attr("data-uk-scrollspy-nav")));
                    }
                });
            });
        },

        init: function() {

            var ids     = [],
                links   = this.find("a[href^='#']").each(function(){ ids.push(ui.$(this).attr("href")); }),
                targets = ui.$(ids.join(",")),

                clsActive  = this.options.cls,
                clsClosest = this.options.closest || this.options.closest;

            var $this = this, inviews, fn = function(){

                inviews = [];

                for (var i=0 ; i < targets.length ; i++) {
                    if (ui.Utils.isInView(targets.eq(i), $this.options)) {
                        inviews.push(targets.eq(i));
                    }
                }

                if (inviews.length) {

                    var navitems,
                        scrollTop = $win.scrollTop(),
                        target = (function(){
                            for(var i=0; i< inviews.length;i++){
                                if(inviews[i].offset().top >= scrollTop){
                                    return inviews[i];
                                }
                            }
                        })();

                    if (!target) return;

                    if ($this.options.closest) {
                        links.closest(clsClosest).removeClass(clsActive);
                        navitems = links.filter("a[href='#"+target.attr("id")+"']").closest(clsClosest).addClass(clsActive);
                    } else {
                        navitems = links.removeClass(clsActive).filter("a[href='#"+target.attr("id")+"']").addClass(clsActive);
                    }

                    $this.element.trigger("inview.uk.scrollspynav", [target, navitems]);
                }
            };

            if (this.options.smoothscroll && ui.smoothScroll) {
                links.each(function(){
                    ui.smoothScroll(this, $this.options.smoothscroll);
                });
            }

            fn();

            this.element.data("scrollspynav", this);

            this.check = fn;
            scrollspynavs.push(this);

        }
    });

})(UIkit);

(function(ui){

    "use strict";

    var toggles = [];

    ui.component('toggle', {

        defaults: {
            target    : false,
            cls       : 'uk-hidden',
            animation : false,
            duration  : 200
        },

        boot: function(){

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-toggle]", context).each(function() {
                    var ele = ui.$(this);

                    if (!ele.data("toggle")) {
                        var obj = ui.toggle(ele, ui.Utils.options(ele.attr("data-uk-toggle")));
                    }
                });

                setTimeout(function(){

                    toggles.forEach(function(toggle){
                        toggle.getToggles();
                    });

                }, 0);
            });
        },

        init: function() {

            var $this = this;

            this.aria = (this.options.cls.indexOf('uk-hidden') !== -1);

            this.getToggles();

            this.on("click", function(e) {
                if ($this.element.is('a[href="#"]')) e.preventDefault();
                $this.toggle();
            });

            toggles.push(this);
        },

        toggle: function() {

            if(!this.totoggle.length) return;

            if (this.options.animation && ui.support.animation) {

                var $this = this, animations = this.options.animation.split(',');

                if (animations.length == 1) {
                    animations[1] = animations[0];
                }

                animations[0] = animations[0].trim();
                animations[1] = animations[1].trim();

                this.totoggle.css('animation-duration', this.options.duration+'ms');

                if (this.totoggle.hasClass(this.options.cls)) {

                    this.totoggle.toggleClass(this.options.cls);

                    this.totoggle.each(function(){
                        ui.Utils.animate(this, animations[0]).then(function(){
                            ui.$(this).css('animation-duration', '');
                            ui.Utils.checkDisplay(this);
                        });
                    });

                } else {

                    this.totoggle.each(function(){
                        ui.Utils.animate(this, animations[1]+' uk-animation-reverse').then(function(){
                            ui.$(this).toggleClass($this.options.cls).css('animation-duration', '');
                            ui.Utils.checkDisplay(this);
                        }.bind(this));
                    });
                }

            } else {
                this.totoggle.toggleClass(this.options.cls);
                ui.Utils.checkDisplay(this.totoggle);
            }

            this.updateAria();

        },

        getToggles: function() {
            this.totoggle = this.options.target ? ui.$(this.options.target):[];
            this.updateAria();
        },

        updateAria: function() {
            if (this.aria && this.totoggle.length) {
                this.totoggle.each(function(){
                    ui.$(this).attr('aria-hidden', ui.$(this).hasClass('uk-hidden'));
                });
            }
        }
    });

})(UIkit);

(function(ui) {

    "use strict";

    ui.component('alert', {

        defaults: {
            "fade": true,
            "duration": 200,
            "trigger": ".uk-alert-close"
        },

        boot: function() {

            // init code
            ui.$html.on("click.alert.uikit", "[data-uk-alert]", function(e) {

                var ele = ui.$(this);

                if (!ele.data("alert")) {

                    var alert = ui.alert(ele, ui.Utils.options(ele.attr("data-uk-alert")));

                    if (ui.$(e.target).is(alert.options.trigger)) {
                        e.preventDefault();
                        alert.close();
                    }
                }
            });
        },

        init: function() {

            var $this = this;

            this.on("click", this.options.trigger, function(e) {
                e.preventDefault();
                $this.close();
            });
        },

        close: function() {

            var element       = this.trigger("close.uk.alert"),
                removeElement = function () {
                    this.trigger("closed.uk.alert").remove();
                }.bind(this);

            if (this.options.fade) {
                element.css("overflow", "hidden").css("max-height", element.height()).animate({
                    "height"         : 0,
                    "opacity"        : 0,
                    "padding-top"    : 0,
                    "padding-bottom" : 0,
                    "margin-top"     : 0,
                    "margin-bottom"  : 0
                }, this.options.duration, removeElement);
            } else {
                removeElement();
            }
        }

    });

})(UIkit);

(function(ui) {

    "use strict";

    ui.component('buttonRadio', {

        defaults: {
            "target": ".uk-button"
        },

        boot: function() {

            // init code
            ui.$html.on("click.buttonradio.uikit", "[data-uk-button-radio]", function(e) {

                var ele = ui.$(this);

                if (!ele.data("buttonRadio")) {

                    var obj    = ui.buttonRadio(ele, ui.Utils.options(ele.attr("data-uk-button-radio"))),
                        target = ui.$(e.target);

                    if (target.is(obj.options.target)) {
                        target.trigger("click");
                    }
                }
            });
        },

        init: function() {

            var $this = this;

            // Init ARIA
            this.find($this.options.target).attr('aria-checked', 'false').filter(".uk-active").attr('aria-checked', 'true');

            this.on("click", this.options.target, function(e) {

                var ele = ui.$(this);

                if (ele.is('a[href="#"]')) e.preventDefault();

                $this.find($this.options.target).not(ele).removeClass("uk-active").blur();
                ele.addClass("uk-active");

                // Update ARIA
                $this.find($this.options.target).not(ele).attr('aria-checked', 'false');
                ele.attr('aria-checked', 'true');

                $this.trigger("change.uk.button", [ele]);
            });

        },

        getSelected: function() {
            return this.find(".uk-active");
        }
    });

    ui.component('buttonCheckbox', {

        defaults: {
            "target": ".uk-button"
        },

        boot: function() {

            ui.$html.on("click.buttoncheckbox.uikit", "[data-uk-button-checkbox]", function(e) {
                var ele = ui.$(this);

                if (!ele.data("buttonCheckbox")) {

                    var obj    = ui.buttonCheckbox(ele, ui.Utils.options(ele.attr("data-uk-button-checkbox"))),
                        target = ui.$(e.target);

                    if (target.is(obj.options.target)) {
                        target.trigger("click");
                    }
                }
            });
        },

        init: function() {

            var $this = this;

            // Init ARIA
            this.find($this.options.target).attr('aria-checked', 'false').filter(".uk-active").attr('aria-checked', 'true');

            this.on("click", this.options.target, function(e) {
                var ele = ui.$(this);

                if (ele.is('a[href="#"]')) e.preventDefault();

                ele.toggleClass("uk-active").blur();

                // Update ARIA
                ele.attr('aria-checked', ele.hasClass("uk-active"));

                $this.trigger("change.uk.button", [ele]);
            });

        },

        getSelected: function() {
            return this.find(".uk-active");
        }
    });


    ui.component('button', {

        defaults: {},

        boot: function() {

            ui.$html.on("click.button.uikit", "[data-uk-button]", function(e) {
                var ele = ui.$(this);

                if (!ele.data("button")) {

                    var obj = ui.button(ele, ui.Utils.options(ele.attr("data-uk-button")));
                    ele.trigger("click");
                }
            });
        },

        init: function() {

            var $this = this;

            // Init ARIA
            this.element.attr('aria-pressed', this.element.hasClass("uk-active"));

            this.on("click", function(e) {

                if ($this.element.is('a[href="#"]')) e.preventDefault();

                $this.toggle();
                $this.trigger("change.uk.button", [$this.element.blur().hasClass("uk-active")]);
            });

        },

        toggle: function() {
            this.element.toggleClass("uk-active");

            // Update ARIA
            this.element.attr('aria-pressed', this.element.hasClass("uk-active"));
        }
    });

})(UIkit);

(function(ui) {

    "use strict";

    var active = false, hoverIdle;

    ui.component('dropdown', {

        defaults: {
           'mode'       : 'hover',
           'remaintime' : 800,
           'justify'    : false,
           'boundary'   : ui.$win,
           'delay'      : 0
        },

        remainIdle: false,

        boot: function() {

            var triggerevent = ui.support.touch ? "click" : "mouseenter";

            // init code
            ui.$html.on(triggerevent+".dropdown.uikit", "[data-uk-dropdown]", function(e) {

                var ele = ui.$(this);

                if (!ele.data("dropdown")) {

                    var dropdown = ui.dropdown(ele, ui.Utils.options(ele.attr("data-uk-dropdown")));

                    if (triggerevent=="click" || (triggerevent=="mouseenter" && dropdown.options.mode=="hover")) {
                        dropdown.element.trigger(triggerevent);
                    }

                    if(dropdown.element.find('.uk-dropdown').length) {
                        e.preventDefault();
                    }
                }
            });
        },

        init: function() {

            var $this = this;

            this.dropdown  = this.find('.uk-dropdown');

            this.centered  = this.dropdown.hasClass('uk-dropdown-center');
            this.justified = this.options.justify ? ui.$(this.options.justify) : false;

            this.boundary  = ui.$(this.options.boundary);
            this.flipped   = this.dropdown.hasClass('uk-dropdown-flip');

            if (!this.boundary.length) {
                this.boundary = ui.$win;
            }

            // Init ARIA
            this.element.attr('aria-haspopup', 'true');
            this.element.attr('aria-expanded', this.element.hasClass("uk-open"));

            if (this.options.mode == "click" || ui.support.touch) {

                this.on("click.uikit.dropdown", function(e) {

                    var $target = ui.$(e.target);

                    if (!$target.parents(".uk-dropdown").length) {

                        if ($target.is("a[href='#']") || $target.parent().is("a[href='#']") || ($this.dropdown.length && !$this.dropdown.is(":visible")) ){
                            e.preventDefault();
                        }

                        $target.blur();
                    }

                    if (!$this.element.hasClass('uk-open')) {

                        $this.show();

                    } else {

                        if ($target.is("a:not(.js-uk-prevent)") || $target.is(".uk-dropdown-close") || !$this.dropdown.find(e.target).length) {
                            $this.hide();
                        }
                    }
                });

            } else {

                this.on("mouseenter", function(e) {

                    if ($this.remainIdle) {
                        clearTimeout($this.remainIdle);
                    }

                    if (hoverIdle) {
                        clearTimeout(hoverIdle);
                    }

                    hoverIdle = setTimeout($this.show.bind($this), $this.options.delay);

                }).on("mouseleave", function() {

                    if (hoverIdle) {
                        clearTimeout(hoverIdle);
                    }

                    $this.remainIdle = setTimeout(function() {
                        $this.hide();
                    }, $this.options.remaintime);

                }).on("click", function(e){

                    var $target = ui.$(e.target);

                    if ($this.remainIdle) {
                        clearTimeout($this.remainIdle);
                    }

                    if ($target.is("a[href='#']") || $target.parent().is("a[href='#']")){
                        e.preventDefault();
                    }

                    $this.show();
                });
            }
        },

        show: function(){

            ui.$html.off("click.outer.dropdown");

            if (active && active[0] != this.element[0]) {
                active.removeClass('uk-open');

                // Update ARIA
                active.attr('aria-expanded', 'false');
            }

            if (hoverIdle) {
                clearTimeout(hoverIdle);
            }

            this.checkDimensions();
            this.element.addClass('uk-open');

            // Update ARIA
            this.element.attr('aria-expanded', 'true');

            this.trigger('show.uk.dropdown', [this]);

            ui.Utils.checkDisplay(this.dropdown, true);
            active = this.element;

            this.registerOuterClick();
        },

        hide: function() {
            this.element.removeClass('uk-open');
            this.remainIdle = false;

            // Update ARIA
            this.element.attr('aria-expanded', 'false');

            if (active && active[0] == this.element[0]) active = false;
        },

        registerOuterClick: function(){

            var $this = this;

            ui.$html.off("click.outer.dropdown");

            setTimeout(function() {

                ui.$html.on("click.outer.dropdown", function(e) {

                    if (hoverIdle) {
                        clearTimeout(hoverIdle);
                    }

                    var $target = ui.$(e.target);

                    if (active && active[0] == $this.element[0] && ($target.is("a:not(.js-uk-prevent)") || $target.is(".uk-dropdown-close") || !$this.dropdown.find(e.target).length)) {
                        $this.hide();
                        ui.$html.off("click.outer.dropdown");
                    }
                });
            }, 10);
        },

        checkDimensions: function() {

            if (!this.dropdown.length) return;

            if (this.justified && this.justified.length) {
                this.dropdown.css("min-width", "");
            }

            var $this     = this,
                dropdown  = this.dropdown.css("margin-" + ui.langdirection, ""),
                offset    = dropdown.show().offset(),
                width     = dropdown.outerWidth(),
                boundarywidth  = this.boundary.width(),
                boundaryoffset = this.boundary.offset() ? this.boundary.offset().left:0;

            // centered dropdown
            if (this.centered) {
                dropdown.css("margin-" + ui.langdirection, (parseFloat(width) / 2 - dropdown.parent().width() / 2) * -1);
                offset = dropdown.offset();

                // reset dropdown
                if ((width + offset.left) > boundarywidth || offset.left < 0) {
                    dropdown.css("margin-" + ui.langdirection, "");
                    offset = dropdown.offset();
                }
            }

            // justify dropdown
            if (this.justified && this.justified.length) {

                var jwidth = this.justified.outerWidth();

                dropdown.css("min-width", jwidth);

                if (ui.langdirection == 'right') {

                    var right1   = boundarywidth - (this.justified.offset().left + jwidth),
                        right2   = boundarywidth - (dropdown.offset().left + dropdown.outerWidth());

                    dropdown.css("margin-right", right1 - right2);

                } else {
                    dropdown.css("margin-left", this.justified.offset().left - offset.left);
                }

                offset = dropdown.offset();

            }

            if ((width + (offset.left-boundaryoffset)) > boundarywidth) {
                dropdown.addClass('uk-dropdown-flip');
                offset = dropdown.offset();
            }

            if ((offset.left-boundaryoffset) < 0) {

                dropdown.addClass("uk-dropdown-stack");

                if (dropdown.hasClass('uk-dropdown-flip')) {

                    if (!this.flipped) {
                        dropdown.removeClass('uk-dropdown-flip');
                        offset = dropdown.offset();
                        dropdown.addClass('uk-dropdown-flip');
                    }

                    setTimeout(function(){

                        if ((dropdown.offset().left-boundaryoffset) < 0 || !$this.flipped && (dropdown.outerWidth() + (offset.left-boundaryoffset)) < boundarywidth) {
                            dropdown.removeClass('uk-dropdown-flip');
                        }
                    }, 0);
                }

                this.trigger('stack.uk.dropdown', [this]);
            }

            dropdown.css("display", "");
        }

    });

})(UIkit);

(function(ui) {

    "use strict";

    var grids = [];

    ui.component('gridMatchHeight', {

        defaults: {
            "target" : false,
            "row"    : true
        },

        boot: function() {

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-grid-match]", context).each(function() {
                    var grid = ui.$(this), obj;

                    if (!grid.data("gridMatchHeight")) {
                        obj = ui.gridMatchHeight(grid, ui.Utils.options(grid.attr("data-uk-grid-match")));
                    }
                });
            });
        },

        init: function() {

            var $this = this;

            this.columns  = this.element.children();
            this.elements = this.options.target ? this.find(this.options.target) : this.columns;

            if (!this.columns.length) return;

            ui.$win.on('load resize orientationchange', (function() {

                var fn = function() {
                    $this.match();
                };

                ui.$(function() { fn(); });

                return ui.Utils.debounce(fn, 50);
            })());

            ui.$html.on("changed.uk.dom", function(e) {
                $this.columns  = $this.element.children();
                $this.elements = $this.options.target ? $this.find($this.options.target) : $this.columns;
                $this.match();
            });

            this.on("display.uk.check", function(e) {
                if(this.element.is(":visible")) this.match();
            }.bind(this));

            grids.push(this);
        },

        match: function() {

            var firstvisible = this.columns.filter(":visible:first");

            if (!firstvisible.length) return;

            var stacked = Math.ceil(100 * parseFloat(firstvisible.css('width')) / parseFloat(firstvisible.parent().css('width'))) >= 100;

            if (stacked) {
                this.revert();
            } else {
                ui.Utils.matchHeights(this.elements, this.options);
            }

            return this;
        },

        revert: function() {
            this.elements.css('min-height', '');
            return this;
        }
    });

    ui.component('gridMargin', {

        defaults: {
            "cls": "uk-grid-margin"
        },

        boot: function() {

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-grid-margin]", context).each(function() {
                    var grid = ui.$(this), obj;

                    if (!grid.data("gridMargin")) {
                        obj = ui.gridMargin(grid, ui.Utils.options(grid.attr("data-uk-grid-margin")));
                    }
                });
            });
        },

        init: function() {

            var stackMargin = ui.stackMargin(this.element, this.options);
        }
    });

    // helper

    ui.Utils.matchHeights = function(elements, options) {

        elements = ui.$(elements).css('min-height', '');
        options  = ui.$.extend({ row : true }, options);

        var matchHeights = function(group){

            if(group.length < 2) return;

            var max = 0;

            group.each(function() {
                max = Math.max(max, ui.$(this).outerHeight());
            }).each(function() {

                var element = ui.$(this),
                height  = max - (element.outerHeight() - element.height());

                element.css('min-height', height + 'px');
            });
        };

        if(options.row) {

            elements.first().width(); // force redraw

            setTimeout(function(){

                var lastoffset = false, group = [];

                elements.each(function() {

                    var ele = ui.$(this), offset = ele.offset().top;

                    if(offset != lastoffset && group.length) {

                        matchHeights(ui.$(group));
                        group  = [];
                        offset = ele.offset().top;
                    }

                    group.push(ele);
                    lastoffset = offset;
                });

                if(group.length) {
                    matchHeights(ui.$(group));
                }

            }, 0);

        } else {
            matchHeights(elements);
        }
    };

})(UIkit);

(function(ui) {

    "use strict";

    var active = false, $html = ui.$html, body;

    ui.component('modal', {

        defaults: {
            keyboard: true,
            bgclose: true,
            minScrollHeight: 150,
            center: false
        },

        scrollable: false,
        transition: false,

        init: function() {

            if (!body) body = ui.$('body');

            var $this = this;

            this.transition = ui.support.transition;
            this.paddingdir = "padding-" + (ui.langdirection == 'left' ? "right":"left");
            this.dialog     = this.find(".uk-modal-dialog");

            // Update ARIA
            this.element.attr('aria-hidden', this.element.hasClass("uk-open"));

            this.on("click", ".uk-modal-close", function(e) {
                e.preventDefault();
                $this.hide();
            }).on("click", function(e) {

                var target = ui.$(e.target);

                if (target[0] == $this.element[0] && $this.options.bgclose) {
                    $this.hide();
                }
            });
        },

        toggle: function() {
            return this[this.isActive() ? "hide" : "show"]();
        },

        show: function() {

            var $this = this;

            if (this.isActive()) return;
            if (active) active.hide(true);

            this.element.removeClass("uk-open").show();
            this.resize();

            active = this;
            $html.addClass("uk-modal-page").height(); // force browser engine redraw

            this.element.addClass("uk-open");

            // Update ARIA
            this.element.attr('aria-hidden', 'false');

            this.element.trigger("show.uk.modal");

            ui.Utils.checkDisplay(this.dialog, true);

            return this;
        },

        hide: function(force) {

            if (!this.isActive()) return;

            if (!force && ui.support.transition) {

                var $this = this;

                this.one(ui.support.transition.end, function() {
                    $this._hide();
                }).removeClass("uk-open");

            } else {

                this._hide();
            }

            return this;
        },

        resize: function() {

            var bodywidth  = body.width();

            this.scrollbarwidth = window.innerWidth - bodywidth;

            body.css(this.paddingdir, this.scrollbarwidth);

            this.element.css('overflow-y', this.scrollbarwidth ? 'scroll' : 'auto');

            if (!this.updateScrollable() && this.options.center) {

                var dh  = this.dialog.outerHeight(),
                pad = parseInt(this.dialog.css('margin-top'), 10) + parseInt(this.dialog.css('margin-bottom'), 10);

                if ((dh + pad) < window.innerHeight) {
                    this.dialog.css({'top': (window.innerHeight/2 - dh/2) - pad });
                } else {
                    this.dialog.css({'top': ''});
                }
            }
        },

        updateScrollable: function() {

            // has scrollable?
            var scrollable = this.dialog.find('.uk-overflow-container:visible:first');

            if (scrollable.length) {

                scrollable.css("height", 0);

                var offset = Math.abs(parseInt(this.dialog.css("margin-top"), 10)),
                dh     = this.dialog.outerHeight(),
                wh     = window.innerHeight,
                h      = wh - 2*(offset < 20 ? 20:offset) - dh;

                scrollable.css("height", h < this.options.minScrollHeight ? "":h);

                return true;
            }

            return false;
        },

        _hide: function() {

            this.element.hide().removeClass("uk-open");

            // Update ARIA
            this.element.attr('aria-hidden', 'true');

            $html.removeClass("uk-modal-page");

            body.css(this.paddingdir, "");

            if(active===this) active = false;

            this.trigger("hide.uk.modal");
        },

        isActive: function() {
            return (active == this);
        }

    });

    ui.component('modalTrigger', {

        boot: function() {

            // init code
            ui.$html.on("click.modal.uikit", "[data-uk-modal]", function(e) {

                var ele = ui.$(this);

                if (ele.is("a")) {
                    e.preventDefault();
                }

                if (!ele.data("modalTrigger")) {
                    var modal = ui.modalTrigger(ele, ui.Utils.options(ele.attr("data-uk-modal")));
                    modal.show();
                }

            });

            // close modal on esc button
            ui.$html.on('keydown.modal.uikit', function (e) {

                if (active && e.keyCode === 27 && active.options.keyboard) { // ESC
                    e.preventDefault();
                    active.hide();
                }
            });

            ui.$win.on("resize orientationchange", ui.Utils.debounce(function(){
                if (active) active.resize();
            }, 150));
        },

        init: function() {

            var $this = this;

            this.options = ui.$.extend({
                "target": $this.element.is("a") ? $this.element.attr("href") : false
            }, this.options);

            this.modal = ui.modal(this.options.target, this.options);

            this.on("click", function(e) {
                e.preventDefault();
                $this.show();
            });

            //methods
            this.proxy(this.modal, "show hide isActive");
        }
    });

    ui.modal.dialog = function(content, options) {

        var modal = ui.modal(ui.$(ui.modal.dialog.template).appendTo("body"), options);

        modal.on("hide.uk.modal", function(){
            if (modal.persist) {
                modal.persist.appendTo(modal.persist.data("modalPersistParent"));
                modal.persist = false;
            }
            modal.element.remove();
        });

        setContent(content, modal);

        return modal;
    };

    ui.modal.dialog.template = '<div class="uk-modal"><div class="uk-modal-dialog" style="min-height:0;"></div></div>';

    ui.modal.alert = function(content, options) {

        ui.modal.dialog(([
            '<div class="uk-margin uk-modal-content">'+String(content)+'</div>',
            '<div class="uk-modal-footer uk-text-right"><button class="uk-button uk-button-primary uk-modal-close">Ok</button></div>'
        ]).join(""), ui.$.extend({bgclose:false, keyboard:false}, options)).show();
    };

    ui.modal.confirm = function(content, onconfirm, options) {

        onconfirm = ui.$.isFunction(onconfirm) ? onconfirm : function(){};

        var modal = ui.modal.dialog(([
            '<div class="uk-margin uk-modal-content">'+String(content)+'</div>',
            '<div class="uk-modal-footer uk-text-right"><button class="uk-button uk-button-primary js-modal-confirm">Ok</button> <button class="uk-button uk-modal-close">Cancel</button></div>'
        ]).join(""), ui.$.extend({bgclose:false, keyboard:false}, options));

        modal.element.find(".js-modal-confirm").on("click", function(){
            onconfirm();
            modal.hide();
        });

        modal.show();
    };


    // helper functions
    function setContent(content, modal){

        if(!modal) return;

        if (typeof content === 'object') {

            // convert DOM object to a jQuery object
            content = content instanceof jQuery ? content : ui.$(content);

            if(content.parent().length) {
                modal.persist = content;
                modal.persist.data("modalPersistParent", content.parent());
            }
        }else if (typeof content === 'string' || typeof content === 'number') {
                // just insert the data as innerHTML
                content = ui.$('<div></div>').html(content);
        }else {
                // unsupported data type!
                content = ui.$('<div></div>').html('UIkit.modal Error: Unsupported data type: ' + typeof content);
        }

        content.appendTo(modal.element.find('.uk-modal-dialog'));

        return modal;
    }

})(UIkit);

(function(ui) {

    "use strict";

    ui.component('nav', {

        defaults: {
            "toggle": ">li.uk-parent > a[href='#']",
            "lists": ">li.uk-parent > ul",
            "multiple": false
        },

        boot: function() {

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-nav]", context).each(function() {
                    var nav = ui.$(this);

                    if (!nav.data("nav")) {
                        var obj = ui.nav(nav, ui.Utils.options(nav.attr("data-uk-nav")));
                    }
                });
            });
        },

        init: function() {

            var $this = this;

            this.on("click.uikit.nav", this.options.toggle, function(e) {
                e.preventDefault();
                var ele = ui.$(this);
                $this.open(ele.parent()[0] == $this.element[0] ? ele : ele.parent("li"));
            });

            this.find(this.options.lists).each(function() {
                var $ele   = ui.$(this),
                    parent = $ele.parent(),
                    active = parent.hasClass("uk-active");

                $ele.wrap('<div style="overflow:hidden;height:0;position:relative;"></div>');
                parent.data("list-container", $ele.parent());

                // Init ARIA
                parent.attr('aria-expanded', parent.hasClass("uk-open"));

                if (active) $this.open(parent, true);
            });

        },

        open: function(li, noanimation) {

            var $this = this, element = this.element, $li = ui.$(li);

            if (!this.options.multiple) {

                element.children(".uk-open").not(li).each(function() {

                    var ele = ui.$(this);

                    if (ele.data("list-container")) {
                        ele.data("list-container").stop().animate({height: 0}, function() {
                            ui.$(this).parent().removeClass("uk-open");
                        });
                    }
                });
            }

            $li.toggleClass("uk-open");

            // Update ARIA
            $li.attr('aria-expanded', $li.hasClass("uk-open"));

            if ($li.data("list-container")) {

                if (noanimation) {
                    $li.data('list-container').stop().height($li.hasClass("uk-open") ? "auto" : 0);
                    this.trigger("display.uk.check");
                } else {
                    $li.data('list-container').stop().animate({
                        height: ($li.hasClass("uk-open") ? getHeight($li.data('list-container').find('ul:first')) : 0)
                    }, function() {
                        $this.trigger("display.uk.check");
                    });
                }
            }
        }
    });


    // helper

    function getHeight(ele) {
        var $ele = ui.$(ele), height = "auto";

        if ($ele.is(":visible")) {
            height = $ele.outerHeight();
        } else {
            var tmp = {
                position: $ele.css("position"),
                visibility: $ele.css("visibility"),
                display: $ele.css("display")
            };

            height = $ele.css({position: 'absolute', visibility: 'hidden', display: 'block'}).outerHeight();

            $ele.css(tmp); // reset element
        }

        return height;
    }

})(UIkit);

(function(ui) {

    "use strict";

    var scrollpos = {x: window.scrollX, y: window.scrollY},
        $win      = ui.$win,
        $doc      = ui.$doc,
        $html     = ui.$html,
        offcanvas = {

        show: function(element) {

            element = ui.$(element);

            if (!element.length) return;

            var $body     = ui.$('body'),
                bar       = element.find(".uk-offcanvas-bar:first"),
                rtl       = (ui.langdirection == "right"),
                flip      = bar.hasClass("uk-offcanvas-bar-flip") ? -1:1,
                dir       = flip * (rtl ? -1 : 1);

            scrollpos = {x: window.pageXOffset, y: window.pageYOffset};

            element.addClass("uk-active");

            $body.css({"width": window.innerWidth, "height": window.innerHeight}).addClass("uk-offcanvas-page");
            $body.css((rtl ? "margin-right" : "margin-left"), (rtl ? -1 : 1) * (bar.outerWidth() * dir)).width(); // .width() - force redraw

            $html.css('margin-top', scrollpos.y * -1);

            bar.addClass("uk-offcanvas-bar-show");

            this._initElement(element);

            $doc.trigger('show.uk.offcanvas', [element, bar]);
            
            // Update ARIA
            element.attr('aria-hidden', 'false');
        },

        hide: function(force) {

            var $body = ui.$('body'),
                panel = ui.$(".uk-offcanvas.uk-active"),
                rtl   = (ui.langdirection == "right"),
                bar   = panel.find(".uk-offcanvas-bar:first"),
                finalize = function() {
                    $body.removeClass("uk-offcanvas-page").css({"width": "", "height": "", "margin-left": "", "margin-right": ""});
                    panel.removeClass("uk-active");

                    bar.removeClass("uk-offcanvas-bar-show");
                    $html.css('margin-top', '');
                    window.scrollTo(scrollpos.x, scrollpos.y);
                    ui.$doc.trigger('hide.uk.offcanvas', [panel, bar]);
                    
                    // Update ARIA
                    panel.attr('aria-hidden', 'true');
                };

            if (!panel.length) return;

            if (ui.support.transition && !force) {

                $body.one(ui.support.transition.end, function() {
                    finalize();
                }).css((rtl ? "margin-right" : "margin-left"), "");

                setTimeout(function(){
                    bar.removeClass("uk-offcanvas-bar-show");
                }, 0);

            } else {
                finalize();
            }
        },

        _initElement: function(element) {

            if (element.data("OffcanvasInit")) return;

            element.on("click.uk.offcanvas swipeRight.uk.offcanvas swipeLeft.uk.offcanvas", function(e) {

                var target = ui.$(e.target);

                if (!e.type.match(/swipe/)) {

                    if (!target.hasClass("uk-offcanvas-close")) {
                        if (target.hasClass("uk-offcanvas-bar")) return;
                        if (target.parents(".uk-offcanvas-bar:first").length) return;
                    }
                }

                e.stopImmediatePropagation();
                offcanvas.hide();
            });

            element.on("click", "a[href^='#']", function(e){

                var element = ui.$(this),
                    href = element.attr("href");

                if (href == "#") {
                    return;
                }

                ui.$doc.one('hide.uk.offcanvas', function() {

                    var target = ui.$(href);

                    if (!target.length) {
                        target = ui.$('[name="'+href.replace('#','')+'"]');
                    }

                    if (ui.Utils.scrollToElement && target.length) {
                        ui.Utils.scrollToElement(target);
                    } else {
                        window.location.href = href;
                    }
                });

                offcanvas.hide();
            });

            element.data("OffcanvasInit", true);
        }
    };

    ui.component('offcanvasTrigger', {

        boot: function() {

            // init code
            $html.on("click.offcanvas.uikit", "[data-uk-offcanvas]", function(e) {

                e.preventDefault();

                var ele = ui.$(this);

                if (!ele.data("offcanvasTrigger")) {
                    var obj = ui.offcanvasTrigger(ele, ui.Utils.options(ele.attr("data-uk-offcanvas")));
                    ele.trigger("click");
                }
            });

            $html.on('keydown.uk.offcanvas', function(e) {

                if (e.keyCode === 27) { // ESC
                    offcanvas.hide();
                }
            });
        },

        init: function() {

            var $this = this;

            this.options = ui.$.extend({
                "target": $this.element.is("a") ? $this.element.attr("href") : false
            }, this.options);

            this.on("click", function(e) {
                e.preventDefault();
                offcanvas.show($this.options.target);
            });
        }
    });

    ui.offcanvas = offcanvas;

})(UIkit);

(function(ui) {

    "use strict";

    var animations;

    ui.component('switcher', {

        defaults: {
            connect   : false,
            toggle    : ">*",
            active    : 0,
            animation : false,
            duration  : 200
        },

        animating: false,

        boot: function() {

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-switcher]", context).each(function() {
                    var switcher = ui.$(this);

                    if (!switcher.data("switcher")) {
                        var obj = ui.switcher(switcher, ui.Utils.options(switcher.attr("data-uk-switcher")));
                    }
                });
            });
        },

        init: function() {

            var $this = this;

            this.on("click.uikit.switcher", this.options.toggle, function(e) {
                e.preventDefault();
                $this.show(this);
            });

            if (this.options.connect) {

                this.connect = ui.$(this.options.connect);

                this.connect.find(".uk-active").removeClass(".uk-active");

                // delegate switch commands within container content
                if (this.connect.length) {

                    // Init ARIA for connect
                    this.connect.children().attr('aria-hidden', 'true');

                    this.connect.on("click", '[data-uk-switcher-item]', function(e) {

                        e.preventDefault();

                        var item = ui.$(this).attr('data-uk-switcher-item');

                        if ($this.index == item) return;

                        switch(item) {
                            case 'next':
                            case 'previous':
                                $this.show($this.index + (item=='next' ? 1:-1));
                                break;
                            default:
                                $this.show(parseInt(item, 10));
                        }
                    }).on('swipeRight swipeLeft', function(e) {
                        e.preventDefault();
                        $this.show($this.index + (e.type == 'swipeLeft' ? 1 : -1));
                    });
                }

                var toggles = this.find(this.options.toggle),
                    active  = toggles.filter(".uk-active");

                if (active.length) {
                    this.show(active, false);
                } else {

                    if (this.options.active===false) return;

                    active = toggles.eq(this.options.active);
                    this.show(active.length ? active : toggles.eq(0), false);
                }

                // Init ARIA for toggles
                toggles.not(active).attr('aria-expanded', 'false');
                active.attr('aria-expanded', 'true');

                this.on('changed.uk.dom', function() {
                    $this.connect = ui.$($this.options.connect);
                });
            }

        },

        show: function(tab, animate) {

            if (this.animating) {
                return;
            }

            if (isNaN(tab)) {
                tab = ui.$(tab);
            } else {

                var toggles = this.find(this.options.toggle);

                tab = tab < 0 ? toggles.length-1 : tab;
                tab = toggles.eq(toggles[tab] ? tab : 0);
            }

            var $this     = this,
                toggles   = this.find(this.options.toggle),
                active    = ui.$(tab),
                animation = animations[this.options.animation] || function(current, next) {

                    if (!$this.options.animation) {
                        return animations.none.apply($this);
                    }

                    var anim = $this.options.animation.split(',');

                    if (anim.length == 1) {
                        anim[1] = anim[0];
                    }

                    anim[0] = anim[0].trim();
                    anim[1] = anim[1].trim();

                    return coreAnimation.apply($this, [anim, current, next]);
                };

            if (animate===false || !ui.support.animation) {
                animation = animations.none;
            }

            if (active.hasClass("uk-disabled")) return;

            // Update ARIA for Toggles
            toggles.attr('aria-expanded', 'false');
            active.attr('aria-expanded', 'true');

            toggles.filter(".uk-active").removeClass("uk-active");
            active.addClass("uk-active");

            if (this.options.connect && this.connect.length) {

                this.index = this.find(this.options.toggle).index(active);

                if (this.index == -1 ) {
                    this.index = 0;
                }

                this.connect.each(function() {

                    var container = ui.$(this),
                        children  = ui.$(container.children()),
                        current   = ui.$(children.filter('.uk-active')),
                        next      = ui.$(children.eq($this.index));

                        $this.animating = true;

                        animation.apply($this, [current, next]).then(function(){

                            current.removeClass("uk-active");
                            next.addClass("uk-active");

                            // Update ARIA for connect
                            current.attr('aria-hidden', 'true');
                            next.attr('aria-hidden', 'false');

                            ui.Utils.checkDisplay(next, true);

                            $this.animating = false;
                        });
                });
            }

            this.trigger("show.uk.switcher", [active]);
        }
    });

    animations = {

        'none': function() {
            var d = ui.$.Deferred();
            d.resolve();
            return d.promise();
        },

        'fade': function(current, next) {
            return coreAnimation.apply(this, ['uk-animation-fade', current, next]);
        },

        'slide-bottom': function(current, next) {
            return coreAnimation.apply(this, ['uk-animation-slide-bottom', current, next]);
        },

        'slide-top': function(current, next) {
            return coreAnimation.apply(this, ['uk-animation-slide-top', current, next]);
        },

        'slide-vertical': function(current, next, dir) {

            var anim = ['uk-animation-slide-top', 'uk-animation-slide-bottom'];

            if (current && current.index() > next.index()) {
                anim.reverse();
            }

            return coreAnimation.apply(this, [anim, current, next]);
        },

        'slide-left': function(current, next) {
            return coreAnimation.apply(this, ['uk-animation-slide-left', current, next]);
        },

        'slide-right': function(current, next) {
            return coreAnimation.apply(this, ['uk-animation-slide-right', current, next]);
        },

        'slide-horizontal': function(current, next, dir) {

            var anim = ['uk-animation-slide-right', 'uk-animation-slide-left'];

            if (current && current.index() > next.index()) {
                anim.reverse();
            }

            return coreAnimation.apply(this, [anim, current, next]);
        },

        'scale': function(current, next) {
            return coreAnimation.apply(this, ['uk-animation-scale-up', current, next]);
        }
    };

    ui.switcher.animations = animations;


    // helpers

    function coreAnimation(cls, current, next) {

        var d = ui.$.Deferred(), clsIn = cls, clsOut = cls, release;

        if (next[0]===current[0]) {
            d.resolve();
            return d.promise();
        }

        if (typeof(cls) == 'object') {
            clsIn  = cls[0];
            clsOut = cls[1] || cls[0];
        }

        release = function() {

            if (current) current.hide().removeClass('uk-active '+clsOut+' uk-animation-reverse');

            next.addClass(clsIn).one(ui.support.animation.end, function() {

                next.removeClass(''+clsIn+'').css({opacity:'', display:''});

                d.resolve();

                if (current) current.css({opacity:'', display:''});

            }.bind(this)).show();
        };

        next.css('animation-duration', this.options.duration+'ms');

        if (current && current.length) {

            current.css('animation-duration', this.options.duration+'ms');

            current.css('display', 'none').addClass(clsOut+' uk-animation-reverse').one(ui.support.animation.end, function() {
                release();
            }.bind(this)).css('display', '');

        } else {
            next.addClass('uk-active');
            release();
        }

        return d.promise();
    }

})(UIkit);

(function(ui) {

    "use strict";

    ui.component('tab', {

        defaults: {
            'target'    : '>li:not(.uk-tab-responsive, .uk-disabled)',
            'connect'   : false,
            'active'    : 0,
            'animation' : false,
            'duration'  : 200
        },

        boot: function() {

            // init code
            ui.ready(function(context) {

                ui.$("[data-uk-tab]", context).each(function() {

                    var tab = ui.$(this);

                    if (!tab.data("tab")) {
                        var obj = ui.tab(tab, ui.Utils.options(tab.attr("data-uk-tab")));
                    }
                });
            });
        },

        init: function() {

            var $this = this;

            this.on("click.uikit.tab", this.options.target, function(e) {
                e.preventDefault();

                if ($this.switcher && $this.switcher.animating) {
                    return;
                }

                var current = $this.find($this.options.target).not(this);

                current.removeClass("uk-active").blur();
                $this.trigger("change.uk.tab", [ui.$(this).addClass("uk-active")]);

                // Update ARIA
                if (!$this.options.connect) {
                    current.attr('aria-expanded', 'false');
                    ui.$(this).attr('aria-expanded', 'true');
                }
            });

            if (this.options.connect) {
                this.connect = ui.$(this.options.connect);
            }

            // init responsive tab
            this.responsivetab = ui.$('<li class="uk-tab-responsive uk-active"><a></a></li>').append('<div class="uk-dropdown uk-dropdown-small"><ul class="uk-nav uk-nav-dropdown"></ul><div>');

            this.responsivetab.dropdown = this.responsivetab.find('.uk-dropdown');
            this.responsivetab.lst      = this.responsivetab.dropdown.find('ul');
            this.responsivetab.caption  = this.responsivetab.find('a:first');

            if (this.element.hasClass("uk-tab-bottom")) this.responsivetab.dropdown.addClass("uk-dropdown-up");

            // handle click
            this.responsivetab.lst.on('click.uikit.tab', 'a', function(e) {

                e.preventDefault();
                e.stopPropagation();

                var link = ui.$(this);

                $this.element.children(':not(.uk-tab-responsive)').eq(link.data('index')).trigger('click');
            });

            this.on('show.uk.switcher change.uk.tab', function(e, tab) {
                $this.responsivetab.caption.html(tab.text());
            });

            this.element.append(this.responsivetab);

            // init UIkit components
            if (this.options.connect) {
                this.switcher = ui.switcher(this.element, {
                    "toggle"    : ">li:not(.uk-tab-responsive)",
                    "connect"   : this.options.connect,
                    "active"    : this.options.active,
                    "animation" : this.options.animation,
                    "duration"  : this.options.duration
                });
            }

            ui.dropdown(this.responsivetab, {"mode": "click"});

            // init
            $this.trigger("change.uk.tab", [this.element.find(this.options.target).filter('.uk-active')]);

            this.check();

            ui.$win.on('resize orientationchange', ui.Utils.debounce(function(){
                if ($this.element.is(":visible"))  $this.check();
            }, 100));

            this.on('display.uk.check', function(){
                if ($this.element.is(":visible"))  $this.check();
            });
        },

        check: function() {

            var children = this.element.children(':not(.uk-tab-responsive)').removeClass('uk-hidden');

            if (!children.length) return;

            var top          = (children.eq(0).offset().top + Math.ceil(children.eq(0).height()/2)),
                doresponsive = false,
                item, link;

            this.responsivetab.lst.empty();

            children.each(function(){

                if (ui.$(this).offset().top > top) {
                    doresponsive = true;
                }
            });

            if (doresponsive) {

                for (var i = 0; i < children.length; i++) {

                    item = ui.$(children.eq(i));
                    link = item.find('a');

                    if (item.css('float') != 'none' && !item.attr('uk-dropdown')) {

                        item.addClass('uk-hidden');

                        if (!item.hasClass('uk-disabled')) {
                            this.responsivetab.lst.append('<li><a href="'+link.attr('href')+'" data-index="'+i+'">'+link.html()+'</a></li>');
                        }
                    }
                }
            }

            this.responsivetab[this.responsivetab.lst.children().length ? 'removeClass':'addClass']('uk-hidden');
        }
    });

})(UIkit);

(function(ui){

    "use strict";

    ui.component('cover', {

        defaults: {
            automute : true
        },

        boot: function() {

            // auto init
            ui.ready(function(context) {

                ui.$("[data-uk-cover]", context).each(function(){

                    var ele = ui.$(this);

                    if(!ele.data("cover")) {
                        var plugin = ui.cover(ele, ui.Utils.options(ele.attr("data-uk-cover")));
                    }
                });
            });
        },

        init: function() {

            this.parent = this.element.parent();

            ui.$win.on('load resize orientationchange', ui.Utils.debounce(function(){
                this.check();
            }.bind(this), 100));

            this.on("display.uk.check", function(e) {
                if(this.element.is(":visible")) this.check();
            }.bind(this));

            this.check();

            if (this.element.is('iframe') && this.options.automute) {

                var src = this.element.attr('src');

                this.element.attr('src', '').on('load', function(){

                    this.contentWindow.postMessage('{ "event": "command", "func": "mute", "method":"setVolume", "value":0}', '*');

                }).attr('src', [src, (src.indexOf('?') > -1 ? '&':'?'), 'enablejsapi=1&api=1'].join(''));
            }
        },

        check: function() {

            this.element.css({
                'width'  : '',
                'height' : ''
            });

            this.dimension = {w: this.element.width(), h: this.element.height()};

            if (this.element.attr('width') && !isNaN(this.element.attr('width'))) {
                this.dimension.w = this.element.attr('width');
            }

            if (this.element.attr('height') && !isNaN(this.element.attr('height'))) {
                this.dimension.h = this.element.attr('height');
            }

            this.ratio     = this.dimension.w / this.dimension.h;

            var w = this.parent.width(), h = this.parent.height(), width, height;

            // if element height < parent height (gap underneath)
            if ((w / this.ratio) < h) {

                width  = Math.ceil(h * this.ratio);
                height = h;

            // element width < parent width (gap to right)
            } else {

                width  = w;
                height = Math.ceil(w / this.ratio);
            }

            this.element.css({
                'width'  : width,
                'height' : height
            });
        }
    });

})(UIkit);
