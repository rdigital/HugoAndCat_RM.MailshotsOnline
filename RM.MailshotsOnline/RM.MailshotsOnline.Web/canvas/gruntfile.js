module.exports = function(grunt) {
  require('load-grunt-tasks')(grunt);

  grunt.initConfig({
    sass: {
        options: {
            sourceMap: true
        },
        dist: {
            files: {
                'styles/compiled/mailshot.css': 'styles/mailshot.scss',
                'styles/compiled/toolbar.css': 'styles/toolbar.scss',
                'styles/compiled/sidepicker.css': 'styles/sidepicker.scss',
                'styles/compiled/canvas.css': 'styles/canvas.scss',
                'styles/compiled/themepicker.css': 'styles/themepicker.scss',
                'styles/compiled/help.css': 'styles/help.scss',
                'styles/compiled/tools.css': 'styles/tools.scss',
                'styles/compiled/preview.css': 'styles/preview.scss',
                'styles/compiled/upload.css': 'styles/upload.scss',
                'styles/compiled/layoutpicker.css': 'styles/layoutpicker.scss',
                'styles/compiled/dropdown.css': 'styles/dropdown.scss',
                'styles/compiled/slider.css': 'styles/slider.scss',
                'styles/compiled/iehacks.css': 'styles/iehacks.scss',
                'styles/compiled/zoom.css': 'styles/zoom.scss',
                'styles/compiled/canvascontrols.css': 'styles/canvascontrols.scss',
                'styles/compiled/auth.css': 'styles/auth.scss'
            }
        }
    },
    watch: {
      css: {
        files: 'styles/**/*.scss',
        tasks: ['sass']
      }
    }
  });

  grunt.loadNpmTasks('grunt-contrib-watch');

  grunt.registerTask('default', ['sass']);
  grunt.registerTask('dev', ['sass', 'watch']);
  //grunt.registerTask('test', ['qunit']);

};