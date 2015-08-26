define(['knockout', 'jquery'],

    function(ko, $) {

        // ViewModel
        function sliderViewModel(params) {
            this.min = params.min;
            this.max = params.max;
            this.current = params.current;
            this.dragging = false;
            this.left_percent = ko.computed(function() {
                return Math.round((this.current() / (this.max - this.min)) * 100)
            }, this)
        }

        /**
         * when slider is dragged, set the value of the current observable
         * according to the min, max and percentage moved
         */
        sliderViewModel.prototype.resize = function resize(data, e) {
            var delta = e.offsetX / $(e.target).width(),
                diff = this.max - this.min;
            this.current(this.min + delta*diff);
        }

        /**
         * handle the drag move event
         */
        sliderViewModel.prototype.dragMove = function dragMove(data, e) {
            if (!this.dragging) {
                return
            }
            this.resize(data, e);
        }

        /**
         * handle the drag start event on the slider (set dragging to true)
         */
        sliderViewModel.prototype.dragStart = function dragStart(data, e) {
            this.dragging = true;
        }

        /**
         * handle the drag end event on the slider (set dragging to false)
         */
        sliderViewModel.prototype.dragEnd = function dragEnd() {
            this.dragging = false;
        }


        return {
            viewModel: sliderViewModel,
            template: { require: 'text!/canvas/templates/slider.html' }
        }
});