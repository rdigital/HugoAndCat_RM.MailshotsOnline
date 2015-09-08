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
                'styles/compiled/preview.css': 'styles/preview.scss'
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