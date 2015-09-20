(function() {
  var animating, objHeight, viewportHeight;

  jQuery.fn.insertFormValidation = function() {
    var $form, changeFormClass, group, input, inputValGroups, _i, _len, _results;
    $form = $(this);
    $form.validateParse();
    changeFormClass = function(inputEl) {
      var el, formGroup;
      el = $(inputEl);
      formGroup = el.closest('.form-group');
      if (el.is(':focus')) {
        formGroup.addClass('has-focus');
      } else {
        formGroup.removeClass('has-focus');
      }
      if (el.val() === '') {
        return formGroup.addClass('is-empty');
      } else {
        return formGroup.removeClass('is-empty');
      }
    };
    $form.find('.form-group:has("textarea")').addClass('text-area-group');
    $form.find('textarea').autogrow();
    $form.find('input[type="radio"]').closest('.form-group').addClass('radio-group');
    $form.find('select').closest('.form-group').addClass('select-group');
    $form.find("input[type='checkbox']").closest('.form-group').addClass('checkbox-group');
    $form.find("input[type='file']").closest('.form-group').addClass('file-group');
    inputValGroups = $form.find('.form-group:has("input"), .form-group:has("textarea")');
    _results = [];
    for (_i = 0, _len = inputValGroups.length; _i < _len; _i++) {
      group = inputValGroups[_i];
      input = $(group).find("input[type='text'], input[type='password'], input[type='email'], input[type='number'], input[type='date'], input[type='time'],textarea");
      input.on('focus', function() {
        return changeFormClass(this);
      });
      input.on('blur', function() {
        return changeFormClass(this);
      });
      _results.push(changeFormClass(input));
    }
    return _results;
  };

  $('form').insertFormValidation();

  objHeight = 0;

  $.each($('form.ms-form').children(":not(:hidden)"), function() {
    return objHeight += $(this).height();
  });

  viewportHeight = document.documentElement.clientHeight || window.innerHeight || 0;

  $('body').height(Math.max(viewportHeight, objHeight));

  $(".action-button").clickRippleEffect();

  animating = false;

  $(".next").click(function(e) {
    var current_fs, input, inputs, next_fs, valid, _i, _len;
    if (animating) {
      return false;
    }
    current_fs = $(this).closest('fieldset');
    inputs = current_fs.find("input:not([type='submit']), textarea");
    valid = true;
    for (_i = 0, _len = inputs.length; _i < _len; _i++) {
      input = inputs[_i];
      if ($(input).valid() === 0) {
        valid = false;
      }
    }
    if (valid === false) {
      return;
    }
    animating = true;
    next_fs = $(this).closest('fieldset').next();
    next_fs.show();
    return current_fs.animate({
      opacity: 0
    }, {
      step: function(now, mx) {
        var left, opacity, scale;
        scale = 1 - (1 - now) * 0.2;
        left = (now * 50) + "%";
        opacity = 1 - now;
        current_fs.css({
          'transform': 'scale(' + scale + ')'
        });
        return next_fs.css({
          'left': left,
          'opacity': opacity
        });
      },
      duration: 800,
      complete: function() {
        current_fs.hide();
        animating = false;
        return $('body').animate({
          scrollTop: 0
        }, 400);
      },
      easing: 'easeInOutBack'
    });
  });

  $('.to-cars').click(function(e) {
    var current_fs, endTimeString, input, inputs, startTimeString, valid, _i, _len;
    current_fs = $(this).closest('fieldset');
    inputs = current_fs.find("input:not([type='submit']), textarea");
    valid = true;
    for (_i = 0, _len = inputs.length; _i < _len; _i++) {
      input = inputs[_i];
      if ($(input).valid() === 0) {
        valid = false;
      }
    }
    if (valid === false) {
      return;
    }
    startTimeString = $('#RequestedStartDate').val() + ' ' + $('#RequestedStartTime').val();
    endTimeString = $('#RequestedEndDate').val() + ' ' + $('#RequestedEndTime').val();
    return $.ajax({
      type: "GET",
      url: "/Vehicle/FreeVehicles?startTime=" + startTimeString + "&endTime=" + endTimeString,
      success: function(data, statusText) {
        if (data !== "") {
          $('.auto-chooser').empty();
          return $('.auto-chooser').append(data);
        }
      }
    });
  });

  $('.previous').click(function(e) {
    var current_fs, previous_fs;
    if (animating) {
      return false;
    }
    animating = true;
    current_fs = $(this).closest('fieldset');
    previous_fs = $(this).closest('fieldset').prev();
    previous_fs.show();
    return current_fs.animate({
      opacity: 0
    }, {
      step: function(now, mx) {
        var left, opacity, scale;
        scale = 0.8 + (1 - now) * 0.2;
        left = ((1 - now) * 50) + "%";
        opacity = 1 - now;
        current_fs.css({
          'left': left
        });
        return previous_fs.css({
          'transform': "scale(" + scale + ")",
          'opacity': opacity
        });
      },
      duration: 800,
      complete: function() {
        current_fs.hide();
        return animating = false;
      },
      easing: 'easeInOutBack'
    });
  });

}).call(this);

//# sourceMappingURL=forms.js.map
