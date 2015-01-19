var bram = {
    init: function () {
        $(document).ready(function () {
            bram.helpers.init();
            bram.signupToggle.init();
        });
    },
    helpers: {
        init: function() {
            bram.helpers.bindShowPasswordToggle();
        },
        bindShowPasswordToggle: function() {
            if ($('.show-password input[type=checkbox]').length) {
                $(document).on('click', '.show-password input', function () {
                    //Because multiple passwords appear on the same page, in the same HTML pattern, find the common parent of one.
                    var commonParent = $(this).parent().parent();
                    if ($(this).prop('checked')) {
                        commonParent.find('input[name="Password"]').attr('type', 'text');
                    } else {
                        commonParent.find('input[name="Password"]').attr('type', 'password');
                    }
                });
            }
        }
    },
    signupToggle: {
        init: function () {
            if ($('.slide-link').length) {
                bram.signupToggle.bind();
            }
        },
        bind: function () {
            $(document).on('click', '.slide-link', function () {
                var slideClass = false;
                // step #1: Get all the classes on the link.
                var linkClasses = $(this).attr('class').split(' ');
                // step #2: Iterate through all the classes to find the one that isn't "slide-link" (there should only ever be two classes).
                for (var i = 0; i < linkClasses.length; i++) {
                    // step #3: If the class isn't "slide-link", it's the one we want.
                    if (linkClasses[i] !== 'slide-link') {
                        var link = linkClasses[i];
                        // step #4: construct the class name of the slide we want to toggle based on the class name of the link we chose.
                        // To do this, we remove out the "-link" and replace it with "-slide".
                        slideClass = '.' + link.split('-link')[0] + '-slide';
                    }
                }
                // step #5: If we've made a slideClass, then activate the toggle for the applicable slide.
                if (slideClass) {
                    bram.signupToggle.toggle(slideClass);
                }
            });
        },
        toggle: function (slideClass) {
            // step #1: Find the slide's parent.
            var parent = $(slideClass).parents('.slide-parent');
            // step #2: Remove the "closed" class from any previously closed slides, the closed to the current open one, and open to the new you want to open.
            parent.find('.slide-toggle.closed').removeClass('closed');
            parent.find('.slide-toggle.open').addClass('closed').removeClass('open');
            $(slideClass).addClass('open');
        }
    },
};

bram.init();