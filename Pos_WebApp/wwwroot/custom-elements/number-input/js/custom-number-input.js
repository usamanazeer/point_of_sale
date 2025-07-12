customNumberInput_INIT();




function customNumberInput_INIT() {
   
    //jQuery('<div class="custom-number-input-nav"><button class="custom-number-input-button custom-number-input-down bg-custom-orange white"><span>-</span></button></div>').insertBefore('.custom-number-input input');
    //jQuery('<div class="custom-number-input-nav"><button class="custom-number-input-button custom-number-input-up bg-custom-orange white"><span>+</span></button></div>').insertAfter('.custom-number-input input');
    setTimeout(function () {
        jQuery('.custom-number-input').each(function () {

            //var btnClasses = $(this)[0].data("btn-classes");

            var spinner = jQuery(this),
                input = spinner.find('input[type="number"]'),
                btnUp = spinner.find('.custom-number-input-up'),
                btnDown = spinner.find('.custom-number-input-down'),
                min = input.attr('min'),
                max = input.attr('max');
            btnUp.click(function (e) {
                if (!e.target.parentElement.parentElement.classList.contains('btns-prevent-default')) {
                    var oldValue = parseFloat(input.val());
                    var newVal = 0;
                    if (max != undefined && max != null) {
                        if (oldValue >= max) {
                            newVal = oldValue;
                        }
                        else {
                            newVal = oldValue + 1;
                        }
                    }
                    else {
                        newVal = oldValue + 1;
                    }
                    spinner.find("input").val(newVal);
                    spinner.find("input").trigger("change");
                };
            });

            btnDown.click(function (e) {
                if (!e.target.parentElement.parentElement.classList.contains('btns-prevent-default')) {
                    var oldValue = parseFloat(input.val());
                    var newVal = 0;
                    if (min != undefined && min != null) {
                        if (oldValue <= min) {
                            newVal = oldValue;
                        }
                        else {
                            newVal = oldValue - 1;
                        }
                    }
                    else {
                        newVal = oldValue - 1;
                    }
                    spinner.find("input").val(newVal);
                    spinner.find("input").trigger("change");
                }
            });
        });
        jQuery('.custom-number-input.disabled').each(function () {
            var spinner = jQuery(this);
            input = spinner.find('input').attr('disabled', 'disabled');
            btnUp = spinner.find('button').attr('disabled', 'disabled');
        });
    },200);
    
}