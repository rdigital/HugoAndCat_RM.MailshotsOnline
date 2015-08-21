module.exports = function(grunt) {
  require('load-grunt-tasks')(grunt);

  grunt.initConfig({
    sass: {
        options: {
            sourceMap: true
        },
        dist: {
            files: {
                'styles/compiled/mailshot.css': 'styles/mailshot.scss'
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

  grunt.registerTask('default', ['sass', 'watch']);
  //grunt.registerTask('test', ['qunit']);

};