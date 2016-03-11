ko.bindingHandlers.scroll = {
    updating: true,

    init: function (element, valueAccessor, allBindingsAccessor) {
        var self = this;
        self.updating = true;
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(window).off("scroll.ko.scrollHandler");
            self.updating = false;
        });
    },

    update: function (element, valueAccessor, allBindingsAccessor) {
        var props = allBindingsAccessor().scrollOptions;
        var offset = props.offset ? props.offset : "0";
        var loadFunc = props.loadFunc;
        var load = ko.utils.unwrapObservable(valueAccessor());
        var self = this;

        if (load) {
            element.style.display = "";
            $(window).on("scroll.ko.scrollHandler", function () {
                var dH = $(document).height();
                var wH = $(window).height();
                var wS = $(window).scrollTop();
                var heightScroll = (dH - offset <= wH + wS);
                if (heightScroll) {
                    if (self.updating) {
                        loadFunc();
                        self.updating = false;
                    }
                } else {
                    self.updating = true;
                }
            });
        } else {
            element.style.display = "none";
            $(window).off("scroll.ko.scrollHandler");
            self.updating = false;
        }
    }
};


ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        $(element).toggle(ko.unwrap(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        ko.unwrap(value) ? $(element).fadeIn(100) : $(element).fadeOut(100);
    }
};

ko.bindingHandlers.fadeVisible = {
    update: function (element, valueAccessor, allBindings) {
        $(window).keydown(function (eventObject) {
            if (eventObject.which == 27)
                $(element).hide();
        });
    }
};

$.fn.animateRotate = function (startAngle, endAngle, duration, easing, complete) {
    return this.each(function () {
        var elem = $(this);

        $({ deg: startAngle }).animate({
            deg: endAngle
        }, {
            duration: duration,
            easing: easing,
            step: function (now) {
                elem.css({
                    '-moz-transform': 'rotate(' + now + 'deg)',
                    '-webkit-transform': 'rotate(' + now + 'deg)',
                    '-o-transform': 'rotate(' + now + 'deg)',
                    '-ms-transform': 'rotate(' + now + 'deg)',
                    'transform': 'rotate(' + now + 'deg)'
                });
            },
            complete: complete || $.noop
        });
    });
};

ko.bindingHandlers.scrollToEl = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var allBindings = allBindingsAccessor();
        var scroll = allBindings.scrollToEl.scroll();
        if (scroll) {
            setTimeout(
                function () {
                    var elem = $(element);
                    if (elem.length > 0) {
                        var offset = elem.offset();
                        if (offset != null) {
                            $(window).scrollTop(offset.top);
                        }
                    }
                }, 0);
        }
    }
};

ko.bindingHandlers.executeOnLeftRight = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var allBindings = allBindingsAccessor();
        $(document).keydown(function (event) {
            var isModalOpened = $("body").hasClass("modal-open");
            if (isModalOpened) {
                var keyCode = (event.which ? event.which : event.keyCode);
                var executeOnLeftRight = allBindings.executeOnLeftRight;
                if (keyCode === 37 && executeOnLeftRight.executeOnLeft != null) {
                    executeOnLeftRight.executeOnLeft.call(viewModel);
                }
                if (keyCode === 39 && executeOnLeftRight.executeOnRight != null) {
                    executeOnLeftRight.executeOnRight.call(viewModel);
                }
            }
            return true;
        });
    }
};

ko.bindingHandlers.executeOnEnter = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var allBindings = allBindingsAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13 && allBindings.executeOnEnter != null) {
                allBindings.executeOnEnter.call(viewModel);
                $(this).blur();
            }
            return true;
        });
    }
};

jQuery.fn.alignCenterScreen = function () {
    this.css("position", "absolute");
    this.css("top", ($(window).height() - this.outerHeight()) / 2 + $(window).scrollTop() + "px");
    this.css("left", ($(window).width() - this.outerWidth()) / 2 + $(window).scrollLeft() + "px");
    return this;
};

jQuery.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) + $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) + $(window).scrollLeft()) + "px");
    return this;
};

jQuery.fn.scrollToElement = function (element, navheight) {
    var offset = element.offset();
    if (offset != null) {
        var offsetTop = offset.top;
        var totalScroll = offsetTop - navheight;
        $('body,html').animate({
            scrollTop: totalScroll
        }, 0);
    }
    return this;
};