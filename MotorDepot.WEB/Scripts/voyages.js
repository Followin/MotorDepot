(function() {
  var backToVoyage;

  $('body').on('click', '.voyage-request', function(e) {
    var $link, id;
    $link = $(e.target);
    if ($link.is('.success')) {
      return;
    }
    $link.addClass('waiting');
    id = $link.data('voyage-id');
    return $.ajax({
      type: "GET",
      url: "/Voyage/RequestVoyage?voyageId=" + id,
      success: function(data, statusText) {
        if (data !== "") {
          if (data === true) {
            $link.removeClass('waiting error').addClass('success');
            return $link.html('Заявка подана');
          } else {
            $link.removeClass('waiting').addClass('error');
            return $link.html('На данный момент, вы не можете подать заявку на данный рейс');
          }
        }
      }
    });
  });

  $('body').on('click', '.to-requests-button', function(e) {
    var $flipContainer, $newblock, $paperBlock, id;
    $paperBlock = $(e.target).closest('.paper-block');
    id = $paperBlock.find("[name='voyageId']").val();
    $flipContainer = $("<div class='flip-container'></div>");
    $newblock = $("<div class='paper-block flip-side'></div>");
    $newblock.height($paperBlock.height());
    $.ajax({
      type: "GET",
      url: "/Voyage/RequestsForVoyage?voyageId=" + id,
      success: function(data, statusText) {
        if (data !== "") {
          return $newblock.append(data);
        }
      }
    });
    $flipContainer.insertBefore($paperBlock);
    $flipContainer.append($paperBlock).append($newblock);
    return $flipContainer.addClass('flipped');
  });

  backToVoyage = function(e) {
    var $container, $flipContainer;
    $container = $(e.target).closest('.flip-side');
    $flipContainer = $container.parent();
    $flipContainer.removeClass('flipped');
    return setTimeout(function() {
      var $mainBlock;
      $mainBlock = $flipContainer.find('.voyage-info');
      $mainBlock.insertBefore($flipContainer);
      return $flipContainer.remove();
    }, 1600);
  };

  $('body').on('click', '.accept-request-button', function(e) {
    var $checked, $container, $flipContainer, $mainContainer, $radios, voyageId;
    $container = $(e.target).closest('.paper-block');
    $flipContainer = $container.parent();
    $mainContainer = $flipContainer.find('.voyage-info');
    voyageId = $flipContainer.find(".voyage-info input[name='voyageId']").val();
    $radios = $container.find(".form-group input[type='radio']");
    $checked = $radios.filter(':checked');
    if ($checked.length > 0) {
      return $.ajax({
        type: "GET",
        url: "/Voyage/AcceptRequest?voyageId=" + voyageId + "&driverId=" + ($checked.val()),
        success: function(data, statusText) {
          if (data === true) {
            return $.ajax({
              type: "GET",
              url: "/Voyage/Details?voyageId=" + voyageId,
              success: function(data, statusText) {
                $mainContainer.empty().append(data);
                return backToVoyage(e);
              }
            });
          }
        }
      });
    }
  });

  $('body').on('click', '.back-to-voyage', backToVoyage);

  $('body').on('change', '.order-select, .status-filter', function() {
    var actionName, orderStr, statusStr;
    orderStr = $('.order-select').val();
    statusStr = $('.status-filter').val();
    actionName = $(this).closest('.filters-block').data('action');
    return $.ajax({
      type: "GET",
      url: "/Voyage/" + actionName + "?sortOrder=" + orderStr + "&status=" + statusStr,
      success: function(data, statusText) {
        return $('.voyages-list').empty().append(data);
      }
    });
  });

}).call(this);

//# sourceMappingURL=voyages.js.map
