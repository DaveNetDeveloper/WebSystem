/**
 * Licensed under the MIT license - http://opensource.org/licenses/MIT
 * Copyright (c) 2014-2018 Osman Nuri Okumuş
 */
'use strict';

if (typeof jQuery === 'undefined') {
  throw new Error('Theme\'s JavaScript requires jQuery');
}

var FullScreen = {

  page: function page(element) {
    $(element).on('click', function (event) {
      event.preventDefault();
      screenfull.toggle();
    });
  },

  box: function box(element) {
    $(element).on('click', function (event) {
      event.preventDefault();
      $(element).toggleClass('fa-window-restore');
      var _target = $(this).parents('.box')[0];
      screenfull.toggle(_target);
    });
  }

};

$(document).on("chl.fullscreen", function () {
  $("[data-fullscreen]").each(function () {
    window.screenfull !== undefined && screenfull.enabled && $(this).is(":visible") && FullScreen[$(this).attr("data-fullscreen")](this);
  });
}).trigger("chl.fullscreen");

var Side = {

  _ps: $('.app-side'),
  _body: $('body'),

  responsive: function responsive() {
    $(window).width() < 768 ? Side._body.removeClass('app-side-mini app-side-opened').addClass('app-side-closed') : Side._body.addClass('app-side-opened').removeClass('app-side-closed');

    if (Side._body.hasClass('page-fixed') & !Side._body.hasClass('app-side-expand-on-hover')) {
      Side._body.removeClass('app-side-mini');
    }
  },

  collapse: function collapse(element) {
    $(element).on('click', function (event) {
      event.preventDefault();
      Side._body.toggleClass('app-side-opened app-side-closed');
      Side._stopMetisMenu();
    });
  },

  mini: function mini(element) {
    $(element).on('click', function (event) {
      event.preventDefault();
      Side._body.toggleClass('app-side-mini');
      Side._stopMetisMenu();
    });
  },

  _stopMetisMenu: function _stopMetisMenu() {
    $('.side-nav').find('li').removeClass('active');
    $('.side-nav').find('a').attr('aria-expanded', false);
    $('.side-nav').find('ul.collapse').removeClass('in').attr('aria-expanded', false);
  }

};

$(document).on("chl.side", function () {
  Side.responsive();
  // var _cb;
  // $(window).on('resize', function () {
  //   _cb = setTimeout(Side.responsive, 100);
  // });

  $("[data-side]").each(function () {
    Side[$(this).attr("data-side")](this);
  });
}).trigger("chl.side");

var Box = {

  collapse: function collapse(element) {
    $(element).on('click', function (event) {
      event.preventDefault();
      var $toggleButtonImage = $(this).children('i.fa');
      var _box = $(this).closest(".box");
      if (_box.has('.box-body')) {
        _box.children('.box-body').on('show.bs.collapse', function () {
          $(element).removeClass('fa-minus fa-plus').addClass('fa-spinner fa-spin');
        }).on('shown.bs.collapse', function () {
          $(element).removeClass('fa-spinner fa-spin').addClass('fa-minus');
        }).on('hide.bs.collapse', function () {
          $(element).removeClass('fa-minus fa-plus').addClass('fa-spinner fa-spin');
        }).on('hidden.bs.collapse', function () {
          $(element).removeClass('fa-spinner fa-spin').addClass('fa-plus');
        }).collapse('toggle');
      }
    });
  },

  close: function close(element) {
    $(element).on('click', function (event) {
      event.preventDefault();
      $(this).closest(".box").hide("slow");
    });
  },

  refresh: function refresh(element) {
    var attrData = $.extend({}, $(element).data());
    var boxBody = $(element).closest(".box").children('.box-body');
    var loaderContainer = $('<div />').addClass('loader hidden');

    var loaderType = attrData.loadersType || 'ball-scale-ripple-multiple';

    var loaders = $('<div />').addClass('loader-inner').addClass(loaderType);
    loaderContainer.append(loaders);
    boxBody.append(loaderContainer);

    $(element).on('click', function (event) {
      event.preventDefault();
      $(element).addClass('fa-spin');
      boxBody.addClass('loaderContainer');
      loaderContainer.removeClass('hidden');
      setTimeout(function () {
        boxBody.removeClass('loaderContainer');
        loaderContainer.addClass('hidden');
        $(element).removeClass('fa-spin');
      }, 2e3);
    });
  }
};

$(document).on("chl.box", function () {
  $("[data-box]").each(function () {
    $(this).is(":visible") && Box[$(this).attr("data-box")](this);
  });
}).trigger("chl.box");

var Pluggin = {
  counterup: function counterup() {
    if ($.fn.counterUp) {
      $('.counterup').counterUp();
    } else {
      throw new Error('First install counterUp plugin! https://github.com/bfintal/Counter-Up');
    }
  },

  metismenu: function metismenu(element) {
    if ($.fn.metisMenu) {
      $(element).metisMenu();
    } else {
      throw new Error('First install metisMenu plugin! https://github.com/onokumus/metisMenu');
    }
  },

  totop: function totop(element) {

    $(window).scroll(function () {
      if ($(this).scrollTop() != 0) {
        $(element).fadeIn();
      } else {
        $(element).fadeOut();
      }
    });

    $(element).on('click', function (event) {
      var _body = $('body');
      event.preventDefault();
      _body.hasClass("page-fixed") ? _body.hasClass("main-fixed") ? $(".main-content").animate({
        scrollTop: 0
      }, 600) : $(".app-main").animate({
        scrollTop: 0
      }, 600) : $("body,html").animate({
        scrollTop: 0
      }, 600);
    });
  }

};

$(document).on("chl.plugin", function () {
  if ($('.counterup').length) {
    Pluggin.counterup({
      delay: 10,
      time: 1000
    });
  }

  $("[data-plugin]").each(function () {
    Pluggin[$(this).attr("data-plugin")](this);
  });
}).trigger("chl.plugin");

$(window).resize(function() {
	if (jQuery(window).width() < 768) {
		jQuery(".app-side-opened.app-side-mini aside.app-side .nav > li > a").css("width", "auto");
	}
});
